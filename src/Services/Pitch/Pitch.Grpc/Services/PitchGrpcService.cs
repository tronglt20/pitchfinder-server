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
        private readonly IUserInfo _userInfo;

        public PitchGrpcService(IPitchRepository pitchRepo, IDistributedCacheRepository distributedCacheRepo, IUserInfo userInfo)
        {
            _pitchRepo = pitchRepo;
            _distributedCacheRepo = distributedCacheRepo;
            _userInfo = userInfo;
        }

        public override async Task<MostSuitablePitchResponse> GetMostSuitablePitch(GetMostSuitablePitchRequest request, ServerCallContext context)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");
            var submittedOrders = await _distributedCacheRepo.GetAsync<List<int>>($"summited-pitchs-by-request-{_userInfo.Id}");

            var mostPuitablePitch = await _pitchRepo.GetMostSuitablePitchAsync(request.StoreId
                , request.Price
                , (PitchTypeEnum)filteringRequest.PitchType
                ,  submittedOrders);

            return new MostSuitablePitchResponse
            {
                PitchId = mostPuitablePitch.Id,
                PitchName = mostPuitablePitch.Name,
                StoreName = mostPuitablePitch.Store.Name,
                Address = mostPuitablePitch.Store.Address,
                Price = mostPuitablePitch.Price,
            };
        }
    }
}
