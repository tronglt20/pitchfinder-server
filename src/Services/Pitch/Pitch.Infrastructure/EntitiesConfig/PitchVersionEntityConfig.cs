using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class PitchVersionEntityConfig : IEntityTypeConfiguration<PitchVersion>
    {
        public void Configure(EntityTypeBuilder<PitchVersion> entity)
        {
            entity.ToTable("PitchVersion");

            entity.HasOne(d => d.Pitch)
                .WithMany(p => p.PitchVersions)
                .HasForeignKey(d => d.PitchId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
