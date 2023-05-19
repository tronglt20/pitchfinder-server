using Pitch.Domain.Entities;
using Pitch.Domain.Interfaces;
using Shared.Infrastructure;

namespace Pitch.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PitchDbContext dbContext) : base(dbContext)
        {
        }
    }
}
