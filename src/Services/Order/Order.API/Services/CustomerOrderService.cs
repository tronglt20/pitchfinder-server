#nullable disable
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Order.Domain.Enums;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using Order.Infrastructure.Dtos;
using PitchFinder.RambitMQ.Events;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public CustomerOrderService(IOrderRepository orderRepo
            , IDistributedCacheRepository distributedCacheRepo
            , IUserInfo userInfo
            , PitchGrpcService pitchGrpcService
            , IUnitOfWorkBase<OrderDbContext> unitOfWorkBase
            , IPublishEndpoint publishEndpoint)
        {
            _orderRepo = orderRepo;
            _distributedCacheRepo = distributedCacheRepo;
            _userInfo = userInfo;
            _pitchGrpcService = pitchGrpcService;
            _unitOfWorkBase = unitOfWorkBase;
            _publishEndpoint = publishEndpoint;
        }

        public async Task ReceivePaymentResultAsync(MomoPaymentResult paymentResult)
        {
            var order = await _orderRepo.GetQuery(_ => _.Id == Int32.Parse( paymentResult.OrderId)).FirstOrDefaultAsync();
            if(order != null && order.Status == OrderStatusEnum.Pending)
            {
                if(paymentResult.Message == "Success")
                {
                    order.Status = OrderStatusEnum.Succesed;
                }

                await _unitOfWorkBase.SaveChangesAsync();
            }

            throw new Exception("Fail to load order");
        }

        public async Task SendPaymentRequestAsync()
        {
            var tempraryOrder = await _distributedCacheRepo.GetAsync<Domain.Entities.Order>($"temporary-order-{_userInfo.Id}");
            await _orderRepo.InsertAsync(tempraryOrder);
            await _unitOfWorkBase.SaveChangesAsync();

            // trigger payment event
            await _publishEndpoint.Publish(new OrderCreatedIntergrationEvent(tempraryOrder.Id, tempraryOrder.Price));
        }

        public async Task<OrderConfirmationResponse> MakeOrderAsync(OrderConfirmationRequest request)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");
            await CachingSubmittedOrderByFilteringRequestAsync(request.StoreId, filteringRequest);

            var mostSuitablePitch = await _pitchGrpcService.GetMostSuitablePitchAsync(request.StoreId, request.Price);
            var result = new OrderConfirmationResponse
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
            await CatchingTemporaryOrderAsync(result);
            return result;
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

        private async Task CatchingTemporaryOrderAsync(OrderConfirmationResponse orderConfirmation)
        {
            var newOrder = new Domain.Entities.Order
            {
                StoreId = orderConfirmation.StoreId,
                PitchId = orderConfirmation.PitchId,
                Status = OrderStatusEnum.Pending,
                Price = orderConfirmation.Price,
                Note = orderConfirmation.Note,
                Date = orderConfirmation.Date,
                Start = orderConfirmation.Start,
                End= orderConfirmation.End,
                CreatedById = _userInfo.Id,
            };
            await _distributedCacheRepo.UpdateAsync<Domain.Entities.Order>($"temporary-order-{_userInfo.Id}", newOrder);

        }
    }
}
