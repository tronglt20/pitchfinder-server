using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class Attachment : BaseAttachment
    {
        public Attachment()
        {

        }

        public virtual ICollection<PitchAttachment> PitchAttachments { get; set; } = new HashSet<PitchAttachment>();
        public virtual ICollection<StoreAttachment> StoreAttachments { get; set; } = new HashSet<StoreAttachment>();
    }
}
