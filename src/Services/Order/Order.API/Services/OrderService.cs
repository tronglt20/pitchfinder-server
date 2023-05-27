using Microsoft.EntityFrameworkCore;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Order.Domain.Enums;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Order.API.Services
{
    public class OrderService
    {
        private readonly IDistributedCacheRepository _distributedCacheRepo;
        private readonly PitchGrpcService _pitchGrpcService;
        private readonly IUnitOfWorkBase _unitOfWorkBase;
        private readonly IOrderRepository _orderRepo;
        private readonly IUserInfo _userInfo;

        public OrderService(IOrderRepository orderRepo
            , IDistributedCacheRepository distributedCacheRepo
            , IUserInfo userInfo
            , PitchGrpcService pitchGrpcService
            , IUnitOfWorkBase<OrderDbContext> unitOfWorkBase)
        {
            _orderRepo = orderRepo;
            _distributedCacheRepo = distributedCacheRepo;
            _userInfo = userInfo;
            _pitchGrpcService = pitchGrpcService;
            _unitOfWorkBase = unitOfWorkBase;
        }

        public async Task<OrderConfirmationResponse> SubmitAsync(OrderConfirmationRequest request)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");
            await CachingSubmittedOrderByFilteringRequestAsync(request.StoreId, filteringRequest);
            var mostSuitablePitch = await _pitchGrpcService.GetMostSuitablePitchAsync(request.StoreId, request.Price);

            // Testing submit Order
            var newOrder = new Domain.Entities.Order()
            {
                StoreId = request.StoreId,
                PitchId = mostSuitablePitch.PitchId,
                PitchType = filteringRequest.PitchType,
                Status = OrderStatusEnum.Succesed,
                Price = mostSuitablePitch.Price,
                Date = filteringRequest.Date,
                Start = filteringRequest.Start,
                End = filteringRequest.End,
                Note = request.Note,
                CreatedById = _userInfo.Id,
                CreatedOn = DateTime.UtcNow,
            };
            await _orderRepo.InsertAsync(newOrder);
            await _unitOfWorkBase.SaveChangesAsync();

            return new OrderConfirmationResponse
            {
                StoreId = request.StoreId,
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
