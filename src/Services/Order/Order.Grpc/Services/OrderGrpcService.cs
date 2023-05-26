using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces;
using Order.Grpc.Protos;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;

namespace Order.Grpc.Services
{
    public class OrderGrpcService : OrderProtoService.OrderProtoServiceBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IDistributedCacheRepository _distributedCacheRepo;

        public OrderGrpcService(IOrderRepository orderRepo, IDistributedCacheRepository distributedCacheRepo)
        {
            _orderRepo = orderRepo;
            _distributedCacheRepo = distributedCacheRepo;
        }

        public override async Task<OrdersByFilteringResponse> GetOrderByFiltering(GetOrdersByFilteringRequest request
            , ServerCallContext context)
        {
            var response = new OrdersByFilteringResponse();
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{request.UserId}");

            var orderItems = await _orderRepo.GetByFilteringRequest(filteringRequest.Date
                , filteringRequest.PitchType
                , filteringRequest.Start
                , filteringRequest.End).Select(_ => new OrderItemByFiltering
                {
                    StoreId = _.StoreId,
                    PitchId = _.PitchId,
                }).ToListAsync();

            response.OrderItem.AddRange(orderItems);
            return response;
        }
    }
}
