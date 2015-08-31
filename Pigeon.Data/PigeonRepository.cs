namespace Pigeon.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
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

        public IQueryable<T> GetAll()
        {
            return this.set;
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> conditions)
        {
            return this.GetAll().Where(conditions);
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entity)
        {
            this.set.Add(entity);
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
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

        public int SaveChanges()
        {
            return this.context.SaveChanges();
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