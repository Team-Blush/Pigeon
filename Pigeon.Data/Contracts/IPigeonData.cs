namespace Pigeon.Data.Contracts
{
    using Pigeon.Models;

    public interface IPigeonData
    {
        IPigeonRepository<User> Users { get; }

        IPigeonRepository<Pigeon> Pigeons { get; }

        IPigeonRepository<Comment> Comments { get; }

        IPigeonRepository<Photo> Photos { get; }

        IPigeonRepository<Notification> Notifications { get; }

        void SaveChanges();
    }
}