#nullable disable
using Microsoft.EntityFrameworkCore;
using Pitch.Domain.Entities;
using System.Reflection;

namespace Pitch.Infrastructure
{
    public class PitchDbContext : DbContext
    {
        public PitchDbContext(DbContextOptions<PitchDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Domain.Entities.Pitch> Pitchs { get; set; }
        public virtual DbSet<PitchVersion> PitchVersions { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<PitchAttachment> PitchAttachment { get; set; }
        public virtual DbSet<StoreRating> StoreRatings { get; set; }
        public virtual DbSet<StoreComment> StoreComments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(PitchDbContext)));
        }
    }
}
