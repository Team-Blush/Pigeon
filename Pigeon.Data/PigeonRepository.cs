﻿using System;
using System.Linq.Expressions;

namespace Pigeon.Data
{
    using System.Data.Entity;
    using System.Linq;
    using Contracts;

    public class PigeonRepository<T> : IPigeonRepository<T>
        where T : class
    {
        private readonly IPigeonContext context;

        private readonly IDbSet<T> set;

        public PigeonRepository()
            : this(new PigeonContext())
        {
        }

        public PigeonRepository(IPigeonContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public void Add(T entity)
        {
            this.set.Add(entity);
        }

        public IQueryable<T> GetAll()
        {
            return this.set;
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> conditions)
        {
            return this.GetAll().Where(conditions);
        }

        public T GetById(int id)
        {
            return this.set.Find(id);
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public void UpdateById(int id)
        {
            var entity = this.set.Find(id);
            this.Update(entity);
        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void DeleteById(int id)
        {
            var entity = this.set.Find(id);
            this.Delete(entity);
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}