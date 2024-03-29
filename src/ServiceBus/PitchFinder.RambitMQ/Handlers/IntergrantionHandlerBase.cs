﻿using MassTransit;
using PitchFinder.RambitMQ.Events;

namespace PitchFinder.RambitMQ.Handlers
{
    public abstract class IntergrantionHandlerBase<T> : IConsumer<T> where T : IntegrationEvent
    {
        public abstract Task Consume(ConsumeContext<T> context);
    }
}
