using Grpc.Core;
using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Pitch.Grpc.Protos;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Pitch.Grpc.Services
{
    public class PitchGrpcService : PitchProtoService.PitchProtoServiceBase
    {
        private readonly IPitchRepository _pitchRepo;
        private readonly IDistributedCacheRepository _distributedCacheRepo;

        public PitchGrpcService(IPitchRepository pitchRepo, IDistributedCacheRepository distributedCacheRepo)
        {
            _pitchRepo = pitchRepo;
            _distributedCacheRepo = distributedCacheRepo;
        }

        public override async Task<MostSuitablePitchResponse> GetMostSuitablePitch(GetMostSuitablePitchRequest request, ServerCallContext context)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{request.UserId}");
            var submittedOrders = await _distributedCacheRepo.GetAsync<List<int>>($"summited-pitchs-by-request-{request.UserId}");

            var mostPuitablePitch = await _pitchRepo.GetMostSuitablePitchAsync(request.StoreId
                , request.Price
                , (PitchTypeEnum)filteringRequest.PitchType
                ,  submittedOrders);

            var result = new MostSuitablePitchResponse
            {
                PitchId = mostPuitablePitch.Id,
                PitchName = mostPuitablePitch.Name,
                StoreName = mostPuitablePitch.Store.Name,
                Address = mostPuitablePitch.Store.Address == null ? "" : mostPuitablePitch.Store.Address,
                Price = mostPuitablePitch.Price,
            };

            return result;
        }
    }
}
