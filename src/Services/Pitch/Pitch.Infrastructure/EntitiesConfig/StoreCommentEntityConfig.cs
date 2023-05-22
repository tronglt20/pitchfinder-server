using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    internal class StoreCommentEntityConfig : IEntityTypeConfiguration<StoreComment>
    {
        public void Configure(EntityTypeBuilder<StoreComment> entity)
        {
            entity.ToTable("StoreComment");

            entity.HasOne(d => d.User)
                .WithMany(p => p.StoreComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Store)
                .WithMany(p => p.StoreComments)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
