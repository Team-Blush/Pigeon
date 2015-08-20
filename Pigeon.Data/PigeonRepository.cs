namespace Pigeon.Data
{
    using System.Data.Entity;
    using System.Linq;

    using Pigeon.Data.Contracts;

    public class PigeonRepository<T> : IPigeonRepository<T>
        where T : class
    {
        private IPigeonContext context;

        private IDbSet<T> set;

        public PigeonRepository()
            : this(new PigeonContext())
        {
        }

        public PigeonRepository(IPigeonContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public void Create(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public IQueryable<T> Read()
        {
            return this.set;
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);

        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);

        }

        public void Detach(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Detached);

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