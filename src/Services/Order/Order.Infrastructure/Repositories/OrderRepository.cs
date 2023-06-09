using Microsoft.EntityFrameworkCore;
using Order.Domain.Enums;
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
                         && _.End == end
                         && _.Status == OrderStatusEnum.Succesed);
        }

        public IQueryable<Domain.Entities.Order> GetByFilteringRequest(DateTime date, int pitchType, TimeSpan start, TimeSpan end)
        {
            return GetQuery(_ => _.Date == date
                         && _.PitchType == pitchType
                         && _.Start == start
                         && _.End == end
                         && _.Status == OrderStatusEnum.Succesed);
        }

        public async Task<List<Domain.Entities.Order>> GetCustomerOrdersAsync(int userId)
        {
            return await GetQuery(_ => _.CreatedById == userId).ToListAsync();
        }

        public async Task<List<Domain.Entities.Order>> GetOwnerOrdersAsync(int storeId)
        {
            return await GetQuery(_ => _.StoreId == storeId && _.Status != OrderStatusEnum.Pending).ToListAsync();
        }
    }
}
