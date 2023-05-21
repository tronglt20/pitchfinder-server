using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class StoreEntityConfig : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> entity)
        {
            entity.ToTable("Store");

            entity.Property(e => e.Address).IsRequired(false);
            entity.Property(e => e.PhoneNumber).IsRequired(false);

            entity.HasOne(d => d.Owner)
                .WithMany(p => p.Stores)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
