using Shared.Domain.Interfaces;

namespace Pitch.Domain.Interfaces
{
    public interface IPitchRepository : IBaseRepository<Domain.Entities.Pitch>
    {
        Task<Domain.Entities.Pitch> GetByNameAsync(string name);
    }
}
