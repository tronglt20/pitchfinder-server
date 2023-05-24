using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Domain.Interfaces;
using Shared.Infrastructure;

namespace Order.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
