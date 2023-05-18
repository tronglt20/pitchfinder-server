using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pitch.Domain.Entities;

namespace Pitch.Infrastructure.EntitiesConfig
{
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");
        }
    }
}
