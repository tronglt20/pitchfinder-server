using Pitch.Domain.Entities;
using Pitch.Domain.Interfaces;
using Shared.Infrastructure;

namespace Pitch.Infrastructure.Repositories
{
    public class StoreRepository : BaseRepository<Store>, IStoreRepository
    {
        public StoreRepository(PitchDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Store> GetAsync(int id)
        {
            return await GetAsync(_ => _.Id == id);
        }
    }
}
