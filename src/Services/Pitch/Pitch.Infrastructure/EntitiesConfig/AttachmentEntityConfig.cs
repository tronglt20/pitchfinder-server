using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class AttachmentEntityConfig : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> entity)
        {
            entity.ToTable("Attachment");
        }
    }
}
