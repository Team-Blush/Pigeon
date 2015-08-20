namespace Pigeon.Data
{
    using System;
    using System.Collections.Generic;

    using Pigeon.Data.Contracts;
    using Pigeon.Models;

    public class PigeonData : IPigeonData
    {
        private IPigeonContext context;

        private IDictionary<Type, object> repositories;

        public PigeonData()
            : this(new PigeonContext())
        {
        }

        public PigeonData(IPigeonContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IPigeonRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IPigeonRepository<Pigeon> Pigeons
        {
            get
            {
                return this.GetRepository<Pigeon>();
            }
        }

        public IPigeonRepository<Comment> Comments
        {
            get
            {
                return this.GetRepository<Comment>();
            }
        }

        public IPigeonRepository<Photo> Photos
        {
            get
            {
                return this.GetRepository<Photo>();
            }
        }

        public IPigeonRepository<Notification> Notifications
        {
            get
            {
                return this.GetRepository<Notification>();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        private IPigeonRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);
            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof(PigeonRepository<T>);

                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return (IPigeonRepository<T>)this.repositories[typeOfModel];
        }
    }
}