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
        private readonly AttachmentService<Attachment, PitchDbContext> _attachmentService;
        private readonly IDistributedCacheRepository _distributedCacheRepo;
        private readonly OrderGrpcService _orderGrpcService;
        private readonly IStoreRepository _storeRepo;
        private readonly IUserInfo _userInfo;

        public StoreOrderingService(IStoreRepository storeRepo
            , AttachmentService<Attachment, PitchDbContext> attachmentService
            , IUserInfo userInfo
            , IDistributedCacheRepository distributedCacheRepo
            , OrderGrpcService orderGrpcService)
        {
            _storeRepo = storeRepo;
            _attachmentService = attachmentService;
            _userInfo = userInfo;
            _distributedCacheRepo = distributedCacheRepo;
            _orderGrpcService = orderGrpcService;
        }

        public async Task<List<StoreOrderingItemResponse>> GetStoresAsync(GetStoreOrderingRequest request)
        {
            // Caching user filtering request
            await _distributedCacheRepo.UpdateAsync<PitchFilteringRequest>
                ($"filtering-request-{_userInfo.Id}", request.GetFilteringRequest());

            var submitedPitchIds = await _orderGrpcService.GetSubmitedPitchIdsAsync();
            var stores = await _storeRepo.GetQuery(request.GetFilter(submitedPitchIds))
                                  .Select(request.GetSelection(submitedPitchIds))
                                  .ToListAsync();

            foreach (var store in stores)
            {
                var pitch = store.Pitch;
                var attachmentKeyName = pitch.PitchAttachments?.Select(_ => _.Attachment.KeyName).FirstOrDefault();
                if (!string.IsNullOrEmpty(attachmentKeyName))
                    store.BackgroundUrl = await _attachmentService.GetPresignedUrl(attachmentKeyName);

                store.Price = pitch.Price;
                store.PitchType = pitch.Type;
            }

            return stores;
        }

        public async Task<PitchFilteringRequest> GetFilteringRequestAsync()
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");
            if (filteringRequest == null)
                throw new Exception("Không có filtering request");
            
            return filteringRequest;
        }
    }
}
