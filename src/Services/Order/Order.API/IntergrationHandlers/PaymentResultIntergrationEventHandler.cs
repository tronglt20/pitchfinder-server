using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Enums;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using PitchFinder.RambitMQ.Events;
using PitchFinder.RambitMQ.Handlers;
using Shared.Domain.Interfaces;

namespace Order.API.IntergrationHandlers
{
    public class PaymentResultIntergrationEventHandler : IntergrantionHandlerBase<PaymentResultIntergrationEvent>
    {

        private readonly IUnitOfWorkBase _unitOfWorkBase;
        private readonly IOrderRepository _orderRepo;

        public PaymentResultIntergrationEventHandler(IUnitOfWorkBase<OrderDbContext> unitOfWorkBase
            , IOrderRepository orderRepo)
        {
            _unitOfWorkBase = unitOfWorkBase;
            _orderRepo = orderRepo;
        }

        public override async Task Consume(ConsumeContext<PaymentResultIntergrationEvent> context)
        {
            var @event = context.Message;
            var order = await _orderRepo.GetQuery(_ => _.Id == Int32.Parse(@event.OrderId)).FirstOrDefaultAsync();
            if (order != null && order.Status == OrderStatusEnum.Pending)
            {
                if (@event.ResultCode == "0")
                    order.Status = OrderStatusEnum.Succesed;
                else
                    order.Status = OrderStatusEnum.Failed;

                await _unitOfWorkBase.SaveChangesAsync();
            }
        }
    }
}
