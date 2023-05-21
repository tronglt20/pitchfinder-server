using Pitch.Domain.Enums;
using Shared.Domain.Entities;
using System.ComponentModel.Design;

namespace Pitch.Domain.Entities
{
    public class Pitch : BaseEntity<int>
    {
        public Pitch()
        {
            
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public PitchStatusEnum Status { get; set; }
        public int Price { get; set; }
        public int StoreId { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<PitchVersion> PitchVersions { get; set; } = new HashSet<PitchVersion>();
        public virtual ICollection<PitchAttachment> PitchAttachments { get; set; } = new HashSet<PitchAttachment>();

        public void UpdateInfo(string name, string description, int price, PitchStatusEnum status)
        {
            Name = name;
            Description = description;
            Price = price;
            Status = status;
        }
    }
}
