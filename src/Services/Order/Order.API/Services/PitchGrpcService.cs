using Pitch.Grpc.Protos;

namespace Order.API.Services
{
    public class PitchGrpcService
    {
        private readonly PitchProtoService.PitchProtoServiceClient _grpcClient;

        public PitchGrpcService(PitchProtoService.PitchProtoServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<MostSuitablePitchResponse> GetMostSuitablePitchAsync(int storeId, int price)
        {
            var request = new GetMostSuitablePitchRequest
            {
                StoreId = storeId,
                Price = price
            };

            try
            {
                return await _grpcClient.GetMostSuitablePitchAsync(request);

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
