﻿#nullable disable
using Microsoft.EntityFrameworkCore;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Order.API.Services
{
    public class CustomerOrderService
    {
        private readonly IDistributedCacheRepository _distributedCacheRepo;
        private readonly PitchGrpcService _pitchGrpcService;
        private readonly IUnitOfWorkBase _unitOfWorkBase;
        private readonly IOrderRepository _orderRepo;
        private readonly IUserInfo _userInfo;

        public CustomerOrderService(IOrderRepository orderRepo
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

        public async Task<List<OrderHistoryItemReponse>> GetOrdersAsync()
        {
            var orders = await _orderRepo.GetCustomerOrdersAsync(_userInfo.Id);
            if (orders == null)
                return null;

            var pichInfo = await _pitchGrpcService.GetPitchInfoAsync(orders);
            var stores = pichInfo.Stores.ToList();
            var pitchs = pichInfo.Pitchs.ToList();
            return orders.Select(_ => new OrderHistoryItemReponse
            {
                OrderId = _.Id,
                StoreId = _.StoreId,
                StoreName = stores.Where(s => s.StoreId == _.StoreId).Select(s => s.StoreName)
                                  .FirstOrDefault(),
                Address = stores.Where(s => s.StoreId == _.StoreId).Select(s => s.Address)
                                  .FirstOrDefault(),
                PitchId = _.PitchId,
                PitchName = pitchs.Where(s => s.PitchId == _.PitchId).Select(s => s.PitchName)
                                  .FirstOrDefault(),
                Price = _.Price,
                Status = _.Status,
                Note = _.Note,
                Date = _.Date,
                Start = _.Start,
                End = _.End,
                CreatedOn = _.CreatedOn,
            }).ToList();
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