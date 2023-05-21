#nullable disable

using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Pitch.API.Services
{
    public class StoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IUnitOfWorkBase<PitchDbContext> _unitOfWorkBase;
        private readonly IUserInfo _userInfo;
        public StoreService(IStoreRepository storeRepo
            , IUnitOfWorkBase<PitchDbContext> unitOfWorkBase
            , IUserInfo userInfo)
        {
            _storeRepo = storeRepo;
            _unitOfWorkBase = unitOfWorkBase;
            _userInfo = userInfo;
        }

        public async Task<StoreUpdateResponse> UpdateStoreInfoAsync(int storeId, StoreUpdateRequest request)
        {
            var store = await _storeRepo.GetAsync(storeId);
            if (store == null)
                throw new Exception("Không tìm thấy store");

            store.UpdateInfo(request.Name
                , request.Address
                , request.PhoneNumber);

            await _unitOfWorkBase.SaveChangesAsync();

            return new StoreUpdateResponse
            {
                Id = store.Id,
                Name = store.Name,
                Address = store.Address,
                PhoneNumber = store.PhoneNumber,
            };
        }

        public async Task<List<PitchItemResponse>> GetPitchsAsync(int storeId)
        {
            var store = await _storeRepo.GetAsync(storeId);
            if (store == null)
                throw new Exception("Không tìm thấy store");

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
    }
}
