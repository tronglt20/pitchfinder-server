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

            try
            {
                return await _grpcClient.GetMostSuitablePitchAsync(request);

            }
            catch (Exception e)
            {

                throw new Exception("Lỗi không tìm thấy sân, vui lòng chọn lại sân khác");
            }
        }

        public async Task<PitchInfoResponse> GetPitchInfoAsync(List<Domain.Entities.Order> orders)
        {
            try
            {
                var request = new GetPitchInfoRequest();
                request.StoreIds.AddRange(orders.Select(_ => _.StoreId).ToList());
                request.PitchIds.AddRange(orders.Select(_ => _.PitchId).ToList());

                return await _grpcClient.GetPitchInfoAsync(request);

            }
            catch (Exception e)
            {

                throw new Exception("Lỗi không tìm thấy sân");
            }
        }
    }
}
