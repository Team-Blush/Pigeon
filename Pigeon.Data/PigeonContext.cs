namespace Pigeon.Data
{
    using System.Data.Entity;

    using Pigeon.Data.Contracts;
    using Pigeon.Data.Migrations;
    using Pigeon.Models;

    public class PigeonContext : DbContext, IPigeonContext
    {
        public PigeonContext()
            : base("PigeonContext")
        {
            var migrationStrategy = new MigrateDatabaseToLatestVersion<PigeonContext, Configuration>();
            Database.SetInitializer(migrationStrategy);
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Pigeon> Pigeons { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public void SaveChanges()
        {
            base.SaveChanges();
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasMany(u => u.ProfilePhoto).WithOptional(p => p.UserProfile).Map(m =>
            {
                m.MapKey("UserProfileId");
            }).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(u => u.CoverPhoto).WithOptional(p => p.UserCover).Map(m =>
            {
                m.MapKey("UserCovetId");
            }).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}