using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.EntitiesConfig
{
    public class OrderEntityConfig : IEntityTypeConfiguration<Domain.Entities.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Order> entity)
        {
            entity.ToTable("Order");

            entity.Property(e => e.Note).IsRequired(false);

            entity.HasOne(d => d.CreatedBy)
               .WithMany(p => p.Orders)
               .HasForeignKey(d => d.CreatedById)
               .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
