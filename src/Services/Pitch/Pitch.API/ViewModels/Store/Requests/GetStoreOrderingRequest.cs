using Pitch.Domain.Enums;
using System.Linq.Expressions;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class GetStoreOrderingRequest
    {

        public PitchTypeEnum Type { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }

        public Expression<Func<Domain.Entities. Store, bool>> Filter()
        {
            return _ => _.Status == StoreStatusEnum.Open
                        && _.Pitchs.Any(p => p.Type == Type && p.Status == PitchStatusEnum.Open)
                        && _.Open <= Open
                        && _.Close >= Close;
        }
    }
}
