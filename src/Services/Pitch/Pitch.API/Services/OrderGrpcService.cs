using Order.Grpc.Protos;
using Shared.Infrastructure.DTOs;

namespace Pitch.API.Services
{
    public class OrderGrpcService
    {
        private readonly OrderProtoService.OrderProtoServiceClient _grpcClient;
        private readonly IUserInfo _userInfo;

        public OrderGrpcService(OrderProtoService.OrderProtoServiceClient grpcClient, IUserInfo userInfo)
        {
            _grpcClient = grpcClient;
            _userInfo = userInfo;
        }

        public async Task<List<int>> GetSubmitedPitchIdsAsync()
        {
            var request = new GetOrdersByFilteringRequest
            {
                UserId = _userInfo.Id,
            };

            try
            {
                var order = await _grpcClient.GetOrderByFilteringAsync(request);
                return order.PitchId.ToList();

            }
            catch (Exception e)
            {

                throw new Exception("Lỗi rồi, check lại đi");
            }
        }
    }
}
