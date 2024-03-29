﻿using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Enums;
using Shared.API.ViewModels;
using System.Linq.Expressions;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class GetStoreOrderingRequest : PitchFilteringRequest
    {
        public Expression<Func<Domain.Entities.Store, bool>> GetFilter(List<int> submitedPitchIds)
        {
            return _ =>
                        !_.Pitchs.Where(p => p.Type == (PitchTypeEnum)PitchType && p.Status == PitchStatusEnum.Open)
                                 .All(p => submitedPitchIds.Contains(p.Id))
                        && _.Status == StoreStatusEnum.Open
                        && _.Open <= Start
                        && _.Close >= End;
        }

        public Expression<Func<Domain.Entities.Store, StoreOrderingItemResponse>> GetSelection(List<int> submitedPitchIds)
        {
            return _ => new StoreOrderingItemResponse
            {
                StoreId = _.Id,
                Name = _.Name,
                Address = _.Address,
                PhoneNumber = _.PhoneNumber,
                Rating = _.StoreRatings.Any() ? (int)_.StoreRatings.Average(_ => _.Rating) : 5,
                Pitch = _.Pitchs.Where(p => !submitedPitchIds.Contains(p.Id) && p.Status == PitchStatusEnum.Open)
                                     .OrderBy(p => p.Price)
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
