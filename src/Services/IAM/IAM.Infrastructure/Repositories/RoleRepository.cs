using IAM.Domain.Entities;
using IAM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;

namespace IAM.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IAMDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await GetAsync(_ => _.Id == id);
        }
    }
}
