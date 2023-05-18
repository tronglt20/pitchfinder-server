#nullable disable
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Pitch.Infrastructure
{
    public class PitchDbContext : DbContext
    {
        public PitchDbContext(DbContextOptions<PitchDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(PitchDbContext)));
        }
    }
}
