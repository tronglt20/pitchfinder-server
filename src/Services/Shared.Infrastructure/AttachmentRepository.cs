using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Shared.Infrastructure
{
    public class AttachmentRepository<T, Tcontext> : BaseRepository<T>, 
                                                     IAttachmentRepository<T, Tcontext> 
                                                        where T : BaseAttachment
                                                        where Tcontext : DbContext
    {
        public AttachmentRepository(Tcontext dbContext) : base(dbContext)
        {
        }
    }
}
