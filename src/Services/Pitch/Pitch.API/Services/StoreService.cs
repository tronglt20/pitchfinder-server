using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using Shared.Domain.Interfaces;

namespace Pitch.API.Services
{
    public class StoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IUnitOfWorkBase<PitchDbContext> _unitOfWorkBase;

        public StoreService(IStoreRepository storeRepo
            , IUnitOfWorkBase<PitchDbContext> unitOfWorkBase)
        {
            _storeRepo = storeRepo;
            _unitOfWorkBase = unitOfWorkBase;
        }

        public async  Task<StoreUpdateResponse> UpdateStoreInfoAsync(int id, StoreUpdateRequest request)
        {
            var store = await _storeRepo.GetAsync(id);
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
    }
}
