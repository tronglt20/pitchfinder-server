using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class StoreRatingEntityConfig : IEntityTypeConfiguration<StoreRating>
    {
        public void Configure(EntityTypeBuilder<StoreRating> entity)
        {
            entity.ToTable("StoreRating");

            entity.HasOne(d => d.User)
                .WithMany(p => p.StoreRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Store)
                .WithMany(p => p.StoreRatings)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
