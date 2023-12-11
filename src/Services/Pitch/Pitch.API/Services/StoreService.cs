#nullable disable

using Castle.Core.Internal;
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

        public async Task<StoreDetailResponse> GetStoreDetailAsync()
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

        public async Task<EditStoreResponse> EditStoreInfoAsync(EditStoreRequest request)
        {
            var store = await GetStoreAsync();
            store.UpdateInfo(request.Name
                , request.Address
                , request.PhoneNumber
                , request.Open
                , request.Close);

            if (request.BackgroundImage != null)
            {
                var image = await _attachmentService.UploadAsync(request.BackgroundImage);
                store.AddAttachment(image);
            }

            await _unitOfWorkBase.SaveChangesAsync();
            return new EditStoreResponse(store);
        }

        public async Task<List<PitchItemResponse>> GetPitchsAsync(string keyName, PitchTypeEnum? pitchType)
        {
            var result = new List<PitchItemResponse>();

            var store = await GetStoreAsync();
            var pitchs = store?.Pitchs.Where(_ => (string.IsNullOrEmpty(keyName) ? true : _.Name.Contains(keyName))
                                                    && (!pitchType.HasValue ? true : _.Type == pitchType));
            foreach ( var pitch in pitchs)
            {
                var pitchModel = new PitchItemResponse()
                {
                    Id = pitch.Id,
                    Name = pitch.Name,
                    Description = pitch.Description,
                    Open = store.Open,
                    Close = store.Close,
                    Status = pitch.Status,
                    Type = pitch.Type,
                    Price = pitch.Price,
                };

                if (!pitch.PitchAttachments.IsNullOrEmpty())
                {
                    foreach (var pitchAttachment in pitch.PitchAttachments)
                    {
                        var attachment = pitchAttachment.Attachment;
                        pitchModel.Attachments.Add(new AttachmentResponse(attachment.Id
                            , attachment.Name
                            , attachment.KeyName
                            , await _attachmentService.GetPresignedUrl(attachment.KeyName)));
                    }
                }
                result.Add(pitchModel);
            }
            return result;
        }

        public async Task AddPitchAsync(AddPitchRequest request)
        {
            var store = await GetStoreAsync();
            var existed = await _pitchRepo.AnyAsync(_ => _.Store.OwnerId == _userInfo.Id && _.Name == request.Name);
            if (existed)
                throw new Exception($"Sân {request.Name} đã tồn tại");

            var newPitch = new Domain.Entities.Pitch
            {
                Store = store,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Type = request.Type,
                Status = PitchStatusEnum.Open,
            };

            if (request?.Formfiles.Count > 0)
            {
                var attachments = await _attachmentService.UploadListAsync(request.Formfiles);
                if (attachments.Count > 0)
                    newPitch.AddAttachments(attachments);
            }

            await _pitchRepo.InsertAsync(newPitch);
            await _unitOfWorkBase.SaveChangesAsync();
        }

        public async Task EditPitchInfoAsync(int pitchId, EditPitchRequest request)
        {
            var pitch = await GetPitchAsync(pitchId);

            pitch.UpdateInfo(request.Name
                , request.Description
                , request.Price
                , request.Type
                , request.Status);

            if (request?.Formfiles.Count > 0)
            {
                var existedAttachs = pitch.PitchAttachments.Select(_ => _.Attachment).ToList();
                if (existedAttachs.Any())
                    await _attachmentService.DeleteListAsync(existedAttachs);

                var attachments = await _attachmentService.UploadListAsync(request.Formfiles);
                if (attachments.Count > 0)
                    pitch.AddAttachments(attachments);
            }
            else
            {
                var existedAttachs = pitch.PitchAttachments.Select(_ => _.Attachment).ToList();
                if (existedAttachs.Any())
                    await _attachmentService.DeleteListAsync(existedAttachs);
            }

            await _unitOfWorkBase.SaveChangesAsync();
        }

        private async Task<Store> GetStoreAsync()
        {
            var store = await _storeRepo.GetAsync(_ => _.OwnerId == _userInfo.Id);
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
