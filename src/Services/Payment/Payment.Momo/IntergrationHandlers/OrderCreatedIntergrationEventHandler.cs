using MassTransit;
using Payment.Momo.Services;
using PitchFinder.RambitMQ.Events;
using PitchFinder.RambitMQ.Handlers;

namespace Payment.Momo.IntergrationHandlers
{
    public class OrderCreatedIntergrationEventHandler : IntergrantionHandlerBase<OrderCreatedIntergrationEvent>
    {
        private readonly MomoService _momoService;

        public OrderCreatedIntergrationEventHandler(MomoService momoService)
        {
            _momoService = momoService;
        }

        public override async Task Consume(ConsumeContext<OrderCreatedIntergrationEvent> context)
        {
            var @event = context.Message;

            await _momoService.PaymentAsync(@event.OrderId.ToString(), @event.Amount.ToString());
        }
    }
}
