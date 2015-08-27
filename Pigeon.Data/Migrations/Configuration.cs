namespace Pigeon.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
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

            if (data.Users.GetAll().Any())
            {
                return;
            }

            var userOne = new User
            {
                UserName = "Pesho123",
                FirstName = "Pesho",
                LastName = "Peshov",
                Age = 23,
                PhoneNumber = "0999765676",
                Email = "pesho123@gmail.com"
            };

            var userTwo = new User
            {
                UserName = "Ivan123",
                FirstName = "Ivan",
                LastName = "Ivanov",
                Age = 18,
                PhoneNumber = "0129765676",
                Email = "ivan123@gmail.com"
            };

            var userThree = new User
            {
                UserName = "Kiro123",
                FirstName = "Kiro",
                LastName = "Kirov",
                Age = 25,
                PhoneNumber = "05459765676",
                Email = "kiro123@gmail.com"
            };

            data.Users.Add(userOne);
            data.Users.Add(userTwo);
            data.Users.Add(userThree);

            data.SaveChanges();
        }
    }
}