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

        public async Task<List<Domain.Entities.Order>> GetOwnerOrdersAsync(int storeId, int? pitchType)
        {
            return await GetQuery(_ => _.StoreId == storeId
                    && _.Status != OrderStatusEnum.Pending
                    && (pitchType.HasValue ? _.PitchType == pitchType : true))
                .ToListAsync();
        }

        public async Task<int> GetRevanueByPitchTypeAsync(int storeId, int pitchType)
        {
            return await GetQuery(_ => _.StoreId == storeId
                  && _.Status == OrderStatusEnum.Succesed
                  && _.PitchType == pitchType)
              .SumAsync(_ => _.Price);
        }

        public async Task<int> GetRevanueByPitchNameAsync(int storeId, int pitchId)
        {
            return await GetQuery(_ => _.StoreId == storeId
                  && _.Status == OrderStatusEnum.Succesed
                  && _.PitchId == pitchId)
              .SumAsync(_ => _.Price);
        }

        public async Task<int> GetRevanueByMonthAsync(DateTime startDate, DateTime endDate)
        {
            return await GetQuery(_ => _.Status == OrderStatusEnum.Succesed
                    && _.CreatedOn >= startDate
                    && _.CreatedOn <= endDate)
              .SumAsync(_ => _.Price);
        }
    }
}
