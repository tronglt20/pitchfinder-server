using MassTransit;
using PitchFinder.RambitMQ.Events;
using PitchFinder.RambitMQ.Handlers;

namespace Pitch.API.IntergrationHandlers
{
    public class UserIntergrationEventHandler : IntergrantionHandlerBase<UserIntergrationEvent>
    {
        public override Task Consume(ConsumeContext<UserIntergrationEvent> context)
        {
            var @event = context.Message;
            throw new NotImplementedException();
        }
    }
}
