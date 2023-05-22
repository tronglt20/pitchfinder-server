using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;

namespace Shared.Domain.Interfaces
{
    public interface IAttachmentRepository<T, Tcontext> : IBaseRepository<T> where T : BaseAttachment
                                                                             where Tcontext : DbContext
    {

    }
}
