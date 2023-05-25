using Grpc.Core;
using Pitch.Domain.Interfaces;
using Pitch.Grpc.Protos;
using Shared.Domain.Interfaces;

namespace Pitch.Grpc.Services
{
    public class PitchGrpcService : PitchProtoService.PitchProtoServiceBase
    {
        private readonly IPitchRepository _pitchRepo;
        private readonly IDistributedCacheRepository _distributedCacheRepo;

        public override Task<MostSuitablePitchResponse> GetMostSuitablePitch(GetMostSuitablePitchRequest request, ServerCallContext context)
        {
            return base.GetMostSuitablePitch(request, context);
        }
    }
}
