using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class PitchEntityConfig : IEntityTypeConfiguration<Domain.Entities.Pitch>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Pitch> entity)
        {
            entity.ToTable("Pitch");

            entity.HasOne(d => d.Store)
                .WithMany(p => p.Pitchs)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
