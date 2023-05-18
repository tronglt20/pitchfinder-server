using IAM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IAM.Infrastructure.EntitiesConfig
{
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");

            entity.Property(u => u.Address)
                .IsRequired(false);

            entity.Property(u => u.AvatarId)
                .IsRequired(false);

            entity.HasOne(d => d.Avatar)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.AvatarId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
