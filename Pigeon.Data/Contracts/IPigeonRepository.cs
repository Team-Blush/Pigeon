namespace Pigeon.Data.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IPigeonRepository<T>
        where T : class
    {
        void Add(T entity);

        IQueryable<T> GetAll();

        IQueryable<T> Search(Expression<Func<T, bool>> conditions);

        T GetById(int id);

        void Update(T entity);

        void UpdateById(int id);

        void Delete(T entity);

        void DeleteById(int id);
    }
}