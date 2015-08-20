﻿namespace Pigeon.Data.Contracts
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Pigeon.Models;

    public interface IPigeonContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Pigeon> Pigeons { get; set; }

        IDbSet<Comment> Comments { get; set; }

        IDbSet<Photo> Photos { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        void SaveChanges();

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntry> Entry<TEntry>(TEntry entry) where TEntry : class;
    }
}