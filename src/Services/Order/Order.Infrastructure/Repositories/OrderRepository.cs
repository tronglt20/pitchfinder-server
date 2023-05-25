using Order.Domain.Interfaces;
using Shared.Infrastructure;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Domain.Entities.Order> GetByFilteringRequest(int storeId, DateTime date, int pitchType, TimeSpan start, TimeSpan end)
        {
            return GetQuery(_ => _.StoreId == storeId
                         && _.Date == date
                         && _.PitchType == pitchType
                         && _.Start == start
                         && _.End == end);
        }
    }
}
