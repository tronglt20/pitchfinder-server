using Microsoft.AspNetCore.Http;
using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class Attachment : BaseEntity<int>
    {
        public Attachment()
        {

        }

        public Attachment(IFormFile formfile, string fileKey)
        {
            Name = formfile.FileName;
            KeyName = fileKey;
            Size = formfile.Length;
            Extension = Path.GetExtension(formfile.FileName);
        }

        public string Name { get; set; }
        public string KeyName { get; set; }
        public float Size { get; set; }
        public string Extension { get; set; }

        public virtual ICollection<PitchAttachment> PitchAttachments { get; set; } = new HashSet<PitchAttachment>();
    }
}
