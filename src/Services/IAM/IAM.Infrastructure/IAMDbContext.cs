using IAM.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IAM.Infrastructure
{
    public class IAMDbContext : IdentityDbContext<User, Role, int>
    {
        public IAMDbContext(DbContextOptions<IAMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Update default Asp table name
            RemoveDefaultAspTableName(builder);
        }

        public void RemoveDefaultAspTableName(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
    }
}
