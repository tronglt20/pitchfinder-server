﻿using MassTransit;
using PitchFinder.RambitMQ.Events;
using PitchFinder.RambitMQ.Handlers;

namespace Pitch.RambitMQ.IntergrationHandlers
{
    public class UserIntergrationEventHandler : IntergrantionHandlerBase<UserIntergrationEvent>
    {
        public override Task Consume(ConsumeContext<UserIntergrationEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
