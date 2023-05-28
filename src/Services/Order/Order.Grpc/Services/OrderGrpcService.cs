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
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{request.UserId}");

            var pitchIds = await _orderRepo.GetByFilteringRequest(filteringRequest.Date
                , filteringRequest.PitchType
                , filteringRequest.Start
                , filteringRequest.End).Select(_ =>_.PitchId).ToListAsync();

            var response = new OrdersByFilteringResponse();
            response.PitchId.AddRange(pitchIds);
            return response;
        }
    }
}
