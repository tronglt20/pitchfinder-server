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
    }
}
