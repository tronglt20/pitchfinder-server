using Shared.Domain.Interfaces;

namespace Order.Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Domain.Entities.Order>
    {
        IQueryable<Domain.Entities.Order> GetByFilteringRequest(int storeId
            , DateTime date
            , int pitchType
            , TimeSpan start
            , TimeSpan end);

        IQueryable<Domain.Entities.Order> GetByFilteringRequest(DateTime date
           , int pitchType
           , TimeSpan start
           , TimeSpan end);

        Task<List<Domain.Entities.Order>> GetCustomerOrdersAsync(int userId);
        Task<List<Domain.Entities.Order>> GetOwnerOrdersAsync(int storeId, int? pitchType);
    }
}
