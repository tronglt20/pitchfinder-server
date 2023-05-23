using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Enums;
using Shared.API.ViewModels;
using System.Linq.Expressions;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class GetStoreOrderingRequest : PitchFilteringRequest
    {
        public Expression<Func<Domain.Entities.Store, bool>> Filter()
        {
            return _ => _.Status == StoreStatusEnum.Open
                        && _.Pitchs.Any(p => p.Type == (PitchTypeEnum)Type && p.Status == PitchStatusEnum.Open)
                        && _.Open <= Open
                        && _.Close >= Close;
        }

        public Expression<Func<Domain.Entities.Store, StoreOrderingItemResponse>> GetSelection()
        {
            return _ => new StoreOrderingItemResponse
            {
                StoreId = _.Id,
                Name = _.Name,
                Address = _.Address,
                PhoneNumber = _.PhoneNumber,
                Rating = _.StoreRatings.Any() ? (int)_.StoreRatings.Average(_ => _.Rating) : 5,
                AttachmentKeyname = _.StoreAttachments.Select(_ => _.Attachment.KeyName).FirstOrDefault(),
                Price = _.Pitchs.Where(p => p.Status == PitchStatusEnum.Open)
                                            .OrderBy(p => p.Price)
                                            .Select(p => p.Price)
                                            .FirstOrDefault(),
            };
        }

        public PitchFilteringRequest GetFilteringRequest()
        {
            return new PitchFilteringRequest
            {
                Date = this.Date,
                Type = this.Type,
                Open = this.Open,
                Close = this.Close,
            };
        }
    }
}
