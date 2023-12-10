using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class PitchAttachment : BaseEntity<int>
    {
        public PitchAttachment()
        {
            
        }

        public PitchAttachment(Pitch pitch, Attachment attachment)
        {
            Pitch = pitch;
            Attachment = attachment;
        }

        public int PitchId { get; set; }
        public int AttachmentId { get; set; }
        public virtual Pitch Pitch { get; set; }
        public virtual Attachment Attachment { get; set; }

    }
}
