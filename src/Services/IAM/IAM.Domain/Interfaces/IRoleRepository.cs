using IAM.Domain.Entities;
using Shared.Domain.Interfaces;

namespace IAM.Domain.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetByIdAsync(int id);
    }
}
