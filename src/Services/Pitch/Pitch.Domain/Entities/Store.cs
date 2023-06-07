using Pitch.Domain.Enums;
using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class Store : BaseEntity<int>
    {
        public Store()
        {

        }

        public string Name { get; set; }
        public string Address { get; set; }
        public StoreStatusEnum Status { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Pitch> Pitchs { get; set; } = new HashSet<Pitch>();
        public virtual ICollection<StoreAttachment> StoreAttachments { get; set; } = new HashSet<StoreAttachment>();
        public virtual ICollection<StoreRating> StoreRatings { get; set; } = new HashSet<StoreRating>();
        public virtual ICollection<StoreComment> StoreComments { get; set; } = new HashSet<StoreComment>();


        public void UpdateInfo(string name, string address, string phoneNumber, TimeSpan open, TimeSpan close)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Open = open;
            Close = close;
        }

        public void AddAttachment(Attachment image)
        {
            this.StoreAttachments.Add(new StoreAttachment(this, image));
        }
    }
}
