#nullable disable
using Microsoft.EntityFrameworkCore;
using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Entities;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using Shared.API.Services;
using Shared.Infrastructure.DTOs;

namespace Pitch.API.Services
{
    public class StoreOrderingService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly AttachmentService<Attachment, PitchDbContext> _attachmentService;
        private readonly IUserInfo _userInfo;

        public StoreOrderingService(IStoreRepository storeRepo
            , AttachmentService<Attachment, PitchDbContext> attachmentService
            , IUserInfo userInfo)
        {
            _storeRepo = storeRepo;
            _attachmentService = attachmentService;
            _userInfo = userInfo;
        }

        public async Task<List<StoreOrderingItemResponse>> GetStoresAsync(GetStoreOrderingRequest request)
        {
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
    }
}
