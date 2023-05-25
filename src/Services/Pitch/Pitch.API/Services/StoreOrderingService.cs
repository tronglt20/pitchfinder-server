#nullable disable
using Microsoft.EntityFrameworkCore;
using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Entities;
using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using Shared.API.Services;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Pitch.API.Services
{
    public class StoreOrderingService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly AttachmentService<Attachment, PitchDbContext> _attachmentService;
        private readonly IUserInfo _userInfo;
        private readonly IDistributedCacheRepository _distributedCacheRepo;

        public StoreOrderingService(IStoreRepository storeRepo
            , AttachmentService<Attachment, PitchDbContext> attachmentService
            , IUserInfo userInfo
            , IDistributedCacheRepository distributedCacheRepo)
        {
            _storeRepo = storeRepo;
            _attachmentService = attachmentService;
            _userInfo = userInfo;
            _distributedCacheRepo = distributedCacheRepo;
        }

        public async Task<List<StoreOrderingItemResponse>> GetStoresAsync(GetStoreOrderingRequest request)
        {
            // Save to cache
            await _distributedCacheRepo.UpdateAsync<PitchFilteringRequest>
                ($"filtering-request-{_userInfo.Id}", request.GetFilteringRequest());

            var stores = await _storeRepo.GetQuery(request.Filter())
                                  .Select(request.GetSelection())
                                  .ToListAsync();

            foreach (var store in stores)
            {
                if (!string.IsNullOrEmpty(store.AttachmentKeyname))
                    store.BackgroundUrl = await _attachmentService.GetPresignedUrl(store.AttachmentKeyname);
            }

            return stores;
        }

        public async Task<StoreOrderingDetailResponse> GetStoreAsync(int id)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");
            if (filteringRequest == null)
                throw new Exception("Không có filtering request");

            var store =  await _storeRepo.GetQuery(_ => _.Id == id)
                        .Select(_ => new StoreOrderingDetailResponse
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
                            Date = filteringRequest.Date,
                            PitchType = filteringRequest.PitchType,
                            Open = filteringRequest.Open,
                            Close = filteringRequest.Close,
                        }).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(store.AttachmentKeyname))
                store.BackgroundUrl = await _attachmentService.GetPresignedUrl(store.AttachmentKeyname);

            return store;
        }
    }
}
