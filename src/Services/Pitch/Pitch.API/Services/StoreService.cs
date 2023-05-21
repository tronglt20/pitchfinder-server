﻿#nullable disable

using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Entities;
using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using PitchFinder.S3.Interfaces;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Pitch.API.Services
{
    public class StoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IPitchRepository _pitchRepo;
        private readonly IUnitOfWorkBase<PitchDbContext> _unitOfWorkBase;
        private readonly IUserInfo _userInfo;
        private readonly IS3Service _s3Service;
        public StoreService(IStoreRepository storeRepo
            , IUnitOfWorkBase<PitchDbContext> unitOfWorkBase
            , IUserInfo userInfo
            , IPitchRepository pitchRepo
            , IS3Service s3Service)
        {
            _storeRepo = storeRepo;
            _unitOfWorkBase = unitOfWorkBase;
            _userInfo = userInfo;
            _pitchRepo = pitchRepo;
            _s3Service = s3Service;
        }

        public async Task<EditStoreResponse> EditStoreInfoAsync(int storeId, EditStoreRequest request)
        {
            var store = await GetStoreAsync(storeId);
            store.UpdateInfo(request.Name
                , request.Address
                , request.PhoneNumber);

            if(request.BackgroundImage != null)
            {
                var image = await _s3Service.UploadAsync(request.BackgroundImage);
                foreach (var attachment in attachments)
                {
                    idea.AddAttachment(attachment);
                }
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
                Description= request.Description,
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
