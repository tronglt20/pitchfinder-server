using Shared.Domain.Interfaces;

namespace Order.Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Domain.Entities.Order>
    {
    }
}
