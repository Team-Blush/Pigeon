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

        public IDbSet<PigeonVote> Votes { get; set; }

        public IDbSet<Pigeon> Pigeons { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Photo> Photos { get; set; }

        public IDbSet<UserSession> UserSessions { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public static PigeonContext Create()
        {
            return new PigeonContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.ProfilePhotos)
                .WithOptional(p => p.ProfilePhotoFor)
                .Map(m => { m.MapKey("UserProfileId"); })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CoverPhotos)
                .WithOptional(p => p.CoverPhotoFor)
                .Map(m => { m.MapKey("UserCoverId"); })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Following)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("FollowingUserId");
                    m.MapRightKey("FollowerUserId");
                    m.ToTable("Following");
                });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("FollowerUserId");
                    m.MapRightKey("FollowingUserId");
                    m.ToTable("Followers");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}