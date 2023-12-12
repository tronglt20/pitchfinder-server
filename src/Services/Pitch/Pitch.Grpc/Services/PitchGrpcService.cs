using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Pitch.Grpc.Protos;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using ZstdSharp.Unsafe;

namespace Pitch.Grpc.Services
{
    public class PitchGrpcService : PitchProtoService.PitchProtoServiceBase
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IPitchRepository _pitchRepo;
        private readonly IDistributedCacheRepository _distributedCacheRepo;

        public PitchGrpcService(IPitchRepository pitchRepo
            , IDistributedCacheRepository distributedCacheRepo
            , IStoreRepository storeRepo)
        {
            _pitchRepo = pitchRepo;
            _distributedCacheRepo = distributedCacheRepo;
            _storeRepo = storeRepo;
        }

        public override async Task<MostSuitablePitchResponse> GetMostSuitablePitch(GetMostSuitablePitchRequest request, ServerCallContext context)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{request.UserId}");
            var submittedOrders = await _distributedCacheRepo.GetAsync<List<int>>($"summited-pitchs-by-request-{request.UserId}");

            var mostPuitablePitch = await _pitchRepo.GetMostSuitablePitchAsync(request.StoreId
                , request.Price
                , (PitchTypeEnum)filteringRequest.PitchType
                , submittedOrders);

            if (mostPuitablePitch == null)
                throw new Exception("Can not find the suitable pitch, try again pleas.");

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

        public override async Task<PitchInfoResponse> GetPitchInfo(GetPitchInfoRequest request, ServerCallContext context)
        {
            var storeIds = request.StoreIds.ToList();
            var pitchIds = request.PitchIds.ToList();

            var stores = await _storeRepo.GetQuery(_ => storeIds.Contains(_.Id))
              .Select(_ => new StoreItemInfoResponse
              {
                  StoreId = _.Id,
                  StoreName = _.Name,
                  Address = _.Address,
              }).ToListAsync();

            var pitchs = await _pitchRepo.GetQuery(_ => pitchIds.Contains(_.Id))
                .Select(_ => new PitchItemInfoResponse
                {
                    PitchId = _.Id,
                    PitchName = _.Name,
                }).ToListAsync();

            var response = new PitchInfoResponse();
            response.Stores.AddRange(stores);
            response.Pitchs.AddRange(pitchs);
            return response;
        }

        public override async Task<PitchInfoResponse> GetOwnerPitchInfo(GetOwnerPitchInfoRequest request, ServerCallContext context)
        {
            var stores = await _storeRepo.GetQuery(_ => _.OwnerId == request.UserId)
              .Select(_ => new StoreItemInfoResponse
              {
                  StoreId = _.Id,
                  StoreName = _.Name,
                  Address = _.Address,
              }).ToListAsync();

            var pitchs = await _pitchRepo.GetQuery(_ => _.Store.OwnerId == request.UserId)
                .Select(_ => new PitchItemInfoResponse
                {
                    PitchId = _.Id,
                    PitchName = _.Name,
                    PitchType = (int)_.Type
                }).ToListAsync();

            var response = new PitchInfoResponse();
            response.Stores.AddRange(stores);
            response.Pitchs.AddRange(pitchs);
            return response;
        }
    }
}
