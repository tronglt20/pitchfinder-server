#nullable disable
using IAM.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IAM.Infrastructure
{
    public class IAMDbContext : IdentityDbContext<User, Role, int>
    {
        public IAMDbContext(DbContextOptions<IAMDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            RemoveDefaultAspTableName(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(IAMDbContext)));
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
