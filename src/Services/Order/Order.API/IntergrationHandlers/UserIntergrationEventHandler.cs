using MassTransit;
using Order.Domain.Entities;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using PitchFinder.RambitMQ.Events;
using PitchFinder.RambitMQ.Handlers;
using Shared.Domain.Interfaces;

namespace Order.API.IntergrationHandlers
{
    public class UserIntergrationEventHandler : IntergrantionHandlerBase<UserAddedIntergrationEvent>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWorkBase<OrderDbContext> _unitOfWorkBase;
        public UserIntergrationEventHandler(IUserRepository userRepo
            , IUnitOfWorkBase<OrderDbContext> unitOfWorkBase)
        {
            _userRepo = userRepo;
            _unitOfWorkBase = unitOfWorkBase;
        }
        public async override Task Consume(ConsumeContext<UserAddedIntergrationEvent> context)
        {
            var @event = context.Message;
            var user = new User(@event.UserId, @event.Email);

            await _userRepo.InsertAsync(user);
            await _unitOfWorkBase.SaveChangesAsync();
        }
    }
}
