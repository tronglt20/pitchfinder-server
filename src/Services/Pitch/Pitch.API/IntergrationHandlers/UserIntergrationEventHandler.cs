using MassTransit;
using Pitch.Domain.Entities;
using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Pitch.Infrastructure;
using PitchFinder.RambitMQ.Events;
using PitchFinder.RambitMQ.Handlers;
using Shared.Domain.Interfaces;

namespace Pitch.API.IntergrationHandlers
{
    public class UserIntergrationEventHandler : IntergrantionHandlerBase<UserAddedIntergrationEvent>
    {
        private readonly IUserRepository _userRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IUnitOfWorkBase<PitchDbContext> _unitOfWorkBase;
        public UserIntergrationEventHandler(IUserRepository userRepo
            , IUnitOfWorkBase<PitchDbContext> unitOfWorkBase
            , IStoreRepository storeRepo)
        {
            _userRepo = userRepo;
            _unitOfWorkBase = unitOfWorkBase;
            _storeRepo = storeRepo;
        }

        public override async Task Consume(ConsumeContext<UserAddedIntergrationEvent> context)
        {
            var @event = context.Message;
            var user = new User(@event.UserId, @event.Email);

            if (!@event.IsCustomer)
            {
                var store = new Store
                {
                    Name = $"Sân - {@event.Email}",
                    Owner = user,
                    Status = StoreStatusEnum.Open,
                    Open = TimeSpan.FromHours(1),
                    Close = TimeSpan.FromHours(23),
                };

                await _storeRepo.InsertAsync(store);
            }

            await _userRepo.InsertAsync(user);
            await _unitOfWorkBase.SaveChangesAsync();
        }
    }
}
