#nullable disable
using Pitch.Grpc.Protos;
using Shared.Infrastructure.DTOs;

namespace Order.API.Services
{
    public class PitchGrpcService
    {
        private readonly PitchProtoService.PitchProtoServiceClient _grpcClient;
        private readonly IUserInfo _userInfo;

        public PitchGrpcService(PitchProtoService.PitchProtoServiceClient grpcClient, IUserInfo userInfo)
        {
            _grpcClient = grpcClient;
            _userInfo = userInfo;
        }

        public async Task<MostSuitablePitchResponse> GetMostSuitablePitchAsync(int storeId, int price)
        {
            var request = new GetMostSuitablePitchRequest
            {
                StoreId = storeId,
                Price = price,
                UserId = _userInfo.Id,
            };
            return await _grpcClient.GetMostSuitablePitchAsync(request);
        }

        public async Task<PitchInfoResponse> GetPitchInfoAsync(List<Domain.Entities.Order> orders)
        {
            var request = new GetPitchInfoRequest();
            request.StoreIds.AddRange(orders.Select(_ => _.StoreId).ToList());
            request.PitchIds.AddRange(orders.Select(_ => _.PitchId).ToList());

            return await _grpcClient.GetPitchInfoAsync(request);
        }

        public async Task<PitchInfoResponse> GetOwnerPitchInfoAsync()
        {
            var request = new GetOwnerPitchInfoRequest()
            {
                UserId = _userInfo.Id,
            };

            return await _grpcClient.GetOwnerPitchInfoAsync(request);
        }
    }
}
