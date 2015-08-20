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
    }
}