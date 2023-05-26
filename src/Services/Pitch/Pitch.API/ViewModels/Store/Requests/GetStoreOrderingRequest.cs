using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Enums;
using Shared.API.ViewModels;
using System.Linq.Expressions;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class GetStoreOrderingRequest : PitchFilteringRequest
    {
        public Expression<Func<Domain.Entities.Store, bool>> Filter(List<SummitedOrderByFilteringResponse> summitedOrder)
        {
            return _ =>
                        (
                            (summitedOrder.Any() && summitedOrder.Any(s => s.StoreId == _.Id))
                            ? _.Pitchs.Where(p => p.Type == (PitchTypeEnum)PitchType && p.Status == PitchStatusEnum.Open)
                                      .Any(p => summitedOrder.Any(s => s.StoreId == _.Id && !s.PitchIds.Contains(p.Id)))
                            : true
                        )
                        && _.Status == StoreStatusEnum.Open
                        && _.Pitchs.Any(p => p.Type == (PitchTypeEnum)PitchType && p.Status == PitchStatusEnum.Open)
                        && _.Open <= Start
                        && _.Close >= End;
        }

        public Expression<Func<Domain.Entities.Store, StoreOrderingItemResponse>> GetSelection(List<SummitedOrderByFilteringResponse> summitedOrder)
        {
            return _ => new StoreOrderingItemResponse
            {
                StoreId = _.Id,
                Name = _.Name,
                Address = _.Address,
                PhoneNumber = _.PhoneNumber,
                Rating = _.StoreRatings.Any() ? (int)_.StoreRatings.Average(_ => _.Rating) : 5,
                AttachmentKeyname = _.StoreAttachments.Select(_ => _.Attachment.KeyName).FirstOrDefault(),
                Price = _.Pitchs.Where(p => 
                                        (!summitedOrder.Any() || summitedOrder.Any(s => s.StoreId == _.Id && !s.PitchIds.Contains(p.Id))) 
                                        && p.Status == PitchStatusEnum.Open
                                      )
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
                PitchType = this.PitchType,
                Start = this.Start,
                End = this.End,
            };
        }
    }
}
