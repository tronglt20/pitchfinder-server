using Pitch.Domain.Enums;
using Pitch.Domain.Interfaces;
using Shared.Infrastructure;

namespace Pitch.Infrastructure.Repositories
{
    public class PitchRepository : BaseRepository<Domain.Entities.Pitch>, IPitchRepository
    {
        public PitchRepository(PitchDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Domain.Entities.Pitch> GetByNameAsync(string name)
        {
            return await GetAsync(_ => _.Name == name);
        }

        public async Task<Domain.Entities.Pitch> GetMostSuitablePitchAsync(int storeId, int price, PitchTypeEnum pitchType, List<int> submittedPitchs)
        {
            return await GetAsync(_ => _.StoreId == storeId
                && (submittedPitchs != null ? !submittedPitchs.Contains(_.Id) : true) 
                && _.Type == pitchType
                && _.Price == price);
        }
    }
}
