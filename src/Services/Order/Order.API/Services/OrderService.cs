using Microsoft.EntityFrameworkCore;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Order.Domain.Interfaces;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Order.API.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IDistributedCacheRepository _distributedCacheRepo;
        private readonly IUserInfo _userInfo;
        private readonly PitchGrpcService _pitchGrpcService;

        public OrderService(IOrderRepository orderRepo
            , IDistributedCacheRepository distributedCacheRepo
            , IUserInfo userInfo
            , PitchGrpcService pitchGrpcService)
        {
            _orderRepo = orderRepo;
            _distributedCacheRepo = distributedCacheRepo;
            _userInfo = userInfo;
            _pitchGrpcService = pitchGrpcService;
        }

        public async Task<OrderConfirmationResponse> SubmitAsync(int storeId, OrderConfirmationRequest request)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");
            await CachingSubmittedOrderByFilteringRequestAsync(storeId, filteringRequest);
            var mostSuitablePitch = await _pitchGrpcService.GetMostSuitablePitchAsync(storeId, request.Price);

            return new OrderConfirmationResponse
            {
                StoreId = storeId,
                StoreName = mostSuitablePitch.StoreName,
                PitchId = mostSuitablePitch.PitchId,
                PitchName = mostSuitablePitch.PitchName,
                Address = mostSuitablePitch.Address,
                PitchType = filteringRequest.PitchType,
                Price = mostSuitablePitch.Price,
                Date = filteringRequest.Date,
                Start = filteringRequest.Start,
                End = filteringRequest.End,
                Note = request.Note,
            };
        }

        private async Task CachingSubmittedOrderByFilteringRequestAsync(int storeId, PitchFilteringRequest filteringRequest)
        {
            var submittedOrders = await _orderRepo.GetByFilteringRequest(storeId
                , filteringRequest.Date
                , filteringRequest.PitchType
                , filteringRequest.Start
                , filteringRequest.End)
                .Select(_ => _.PitchId).ToListAsync();

            await _distributedCacheRepo.UpdateAsync<List<int>>($"summited-pitchs-by-request-{_userInfo.Id}", submittedOrders);
        }
    }
}
