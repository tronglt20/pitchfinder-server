using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class StoreAttachment : BaseEntity<int>
    {

        public StoreAttachment()
        {

        }

        public StoreAttachment(Store store, Attachment attachment)
        {
            Store = store;
            Attachment = attachment;
        }

        public int StoreId { get; set; }
        public int AttachmentId { get; set; }
        public virtual Store Store { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
