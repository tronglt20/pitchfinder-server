using MassTransit;
using Pitch.Domain.Entities;
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
        private readonly IUnitOfWorkBase<PitchDbContext> _unitOfWorkBase;
        public UserIntergrationEventHandler(IUserRepository userRepo
            , IUnitOfWorkBase<PitchDbContext> unitOfWorkBase)
        {
            _userRepo = userRepo;
            _unitOfWorkBase = unitOfWorkBase;
        }

        public override async Task Consume(ConsumeContext<UserAddedIntergrationEvent> context)
        {
            var @event = context.Message;

            await _userRepo.InsertAsync(new User(@event.UserId, @event.Email));

            await _unitOfWorkBase.SaveChangesAsync();
        }
    }
}
