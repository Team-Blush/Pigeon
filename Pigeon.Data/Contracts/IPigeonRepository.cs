namespace Pigeon.Data.Contracts
{
    using System.Linq;

    public interface IPigeonRepository<T>
        where T : class
    {
        void Add(T entity);

        IQueryable<T> GetAll();

        T GetById(int id);

        void Update(T entity);

        void UpdateById(int id);

        void Delete(T entity);

        void DeleteById(int id);
    }
}