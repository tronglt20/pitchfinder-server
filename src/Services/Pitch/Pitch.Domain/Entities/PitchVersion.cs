using Pitch.Domain.Enums;
using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class PitchVersion : BaseEntity<int>
    {
        public PitchVersion()
        {

        }

        public string Name { get; set; }
        public PitchStatusEnum Status { get; set; }
        public int Price { get; set; }
        public float Duration { get; set; }
        public int PitchId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Pitch Pitch { get; set; }
    }
}
