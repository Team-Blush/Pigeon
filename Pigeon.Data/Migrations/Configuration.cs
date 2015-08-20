namespace Pigeon.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PigeonContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PigeonContext context)
        {
            var data = new PigeonData();
            var notification = new Notification()
            {
                Content = "First Notification!",
                Type = NotificationType.PigeonFavourited
            };
            data.Notifications.Create(notification);

            data.SaveChanges();
        }
    }
}