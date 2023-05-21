using Pitch.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Pitch.Domain.Interfaces
{
    public interface IStoreRepository : IBaseRepository<Store>
    {
        Task<Store> GetAsync(int id);
        IQueryable<Domain.Entities.Pitch> GetPitchs(int id);
    }
}
