namespace Pigeon.Data.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IPigeonRepository<T>
        where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> Search(Expression<Func<T, bool>> conditions);

        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteById(int id);

        int SaveChanges();
    }
}