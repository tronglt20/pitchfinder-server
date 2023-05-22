using Microsoft.AspNetCore.Http;
using Shared.Domain.Entities;

namespace IAM.Domain.Entities
{
    public class Attachment : BaseAttachment
    {
        public Attachment()
        {

        }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
