#nullable disable

using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Entities;
using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using Shared.API.Services;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Pitch.API.Services
{
    public class StoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IPitchRepository _pitchRepo;
        private readonly IUnitOfWorkBase<PitchDbContext> _unitOfWorkBase;
        private readonly AttachmentService<Attachment, PitchDbContext> _attachmentService;
        private readonly IUserInfo _userInfo;

        public StoreService(IStoreRepository storeRepo
            , IUnitOfWorkBase<PitchDbContext> unitOfWorkBase
            , IPitchRepository pitchRepo
            , AttachmentService<Attachment, PitchDbContext> attachmentService, IUserInfo userInfo)
        {
            _storeRepo = storeRepo;
            _unitOfWorkBase = unitOfWorkBase;
            _pitchRepo = pitchRepo;
            _attachmentService = attachmentService;
            _userInfo = userInfo;
        }

        public async Task<StoreDetailResponse> GetStoreAsync()
        {
            var store = await _storeRepo.GetAsync(_ => _.OwnerId == _userInfo.Id);
            if (store == null)
                throw new Exception("Không tìm thấy store");

            var response = new StoreDetailResponse(store);
            if (store.StoreAttachments.Any())
            {
                var backgroundAttachment = store.StoreAttachments.FirstOrDefault();
                response.BackgroundUrl = await _attachmentService.GetPresignedUrl(backgroundAttachment.Attachment.KeyName);
            }

            return response;
        }

        public async Task<EditStoreResponse> EditStoreInfoAsync(int storeId, EditStoreRequest request)
        {
            var store = await GetStoreAsync(storeId);
            store.UpdateInfo(request.Name
                , request.Address
                , request.PhoneNumber);

            if (request.BackgroundImage != null)
            {
                var image = await _attachmentService.UploadAsync(request.BackgroundImage);
                store.AddAttachment(image);
            }

            await _unitOfWorkBase.SaveChangesAsync();
            return new EditStoreResponse(store);
        }

        public async Task<List<PitchItemResponse>> GetPitchsAsync(int storeId)
        {
            var store = await GetStoreAsync(storeId);
            var pitchs = store.Pitchs?.ToList();
            return pitchs?.Select(_ => new PitchItemResponse
            {
                Name = _.Name,
                Description = _.Description,
                Open = store.Open,
                Close = store.Close,
                Status = _.Status,
            }).ToList();
        }

        public async Task AddPitchAsync(int storeId, AddPitchRequest request)
        {
            var store = await GetStoreAsync(storeId);
            var existed = await _pitchRepo.AnyAsync(_ => _.Name == request.Name);
            if (existed)
                throw new Exception($"Sân {request.Name} đã tồn tại");

            var newPitch = new Domain.Entities.Pitch
            {
                Store = store,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Status = PitchStatusEnum.Open
            };

            await _pitchRepo.InsertAsync(newPitch);
            await _unitOfWorkBase.SaveChangesAsync();
        }

        public async Task EditPitchInfoAsync(int pitchId, EditPitchRequest request)
        {
            var pitch = await GetPitchAsync(pitchId);

            pitch.UpdateInfo(request.Name
                , request.Description
                , request.Price
                , request.Status);
            await _unitOfWorkBase.SaveChangesAsync();
        }

        private async Task<Store> GetStoreAsync(int storeId)
        {
            var store = await _storeRepo.GetAsync(storeId);
            if (store == null)
                throw new Exception("Không tìm thấy store");

            return store;
        }

        private async Task<Domain.Entities.Pitch> GetPitchAsync(int pitchId)
        {
            var pitch = await _pitchRepo.GetAsync(_ => _.Id == pitchId);
            if (pitch == null)
                throw new Exception("Không tìm thấy pitch");

            return pitch;
        }
    }
}
