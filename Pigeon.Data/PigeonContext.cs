namespace Pigeon.Data
{
    using System.Data.Entity;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;

    public class PigeonContext : IdentityDbContext<User>, IPigeonContext
    {
        public PigeonContext()
            : base("PigeonContext")
        {
            var migrationStrategy = new MigrateDatabaseToLatestVersion<PigeonContext, Configuration>();
            Database.SetInitializer(migrationStrategy);
        }

        // IdentityContext already has this
        //public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Pigeon> Pigeons { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public static PigeonContext Create()
        {
            return new PigeonContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.ProfilePhoto)
                .WithOptional(p => p.ProfilePhotoFor)
                .Map(m => { m.MapKey("UserProfileId"); })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CoverPhoto)
                .WithOptional(p => p.CoverPhotoFor)
                .Map(m => { m.MapKey("UserCovetId"); })
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}