using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    internal class StoreAttachmentEntityConfig : IEntityTypeConfiguration<StoreAttachment>
    {
        public void Configure(EntityTypeBuilder<StoreAttachment> entity)
        {
            entity.ToTable("StoreAttachment");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.StoreAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Store)
                .WithMany(p => p.StoreAttachments)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
