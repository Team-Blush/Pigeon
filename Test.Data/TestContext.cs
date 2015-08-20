namespace Test.Data
{
    using System.Data.Entity;

    using Pigeon.Models;

    using Test.Data.Migrations;

    public class TestContext : DbContext
    {
        public TestContext()
            : base("TestContext")
        {
            var migrationStrategy = new MigrateDatabaseToLatestVersion<TestContext, Configuration>();
            Database.SetInitializer(migrationStrategy);
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Pigeon> Pigeons { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Photo> Photos { get; set; }

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