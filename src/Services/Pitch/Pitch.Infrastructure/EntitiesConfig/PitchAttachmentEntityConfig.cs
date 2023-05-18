using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class PitchAttachmentEntityConfig : IEntityTypeConfiguration<PitchAttachment>
    {
        public void Configure(EntityTypeBuilder<PitchAttachment> entity)
        {
            entity.ToTable("PitchAttachment");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.PitchAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Pitch)
                .WithMany(p => p.PitchAttachments)
                .HasForeignKey(d => d.PitchId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
