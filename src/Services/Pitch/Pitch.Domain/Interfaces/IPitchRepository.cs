using Pitch.Domain.Enums;
using Shared.Domain.Interfaces;

namespace Pitch.Domain.Interfaces
{
    public interface IPitchRepository : IBaseRepository<Domain.Entities.Pitch>
    {
        Task<Domain.Entities.Pitch> GetByNameAsync(string name);
        Task<Domain.Entities.Pitch> GetMostSuitablePitchAsync(int storeId, int price, PitchTypeEnum pitchType, List<int> submittedPitchs);
    }
}
