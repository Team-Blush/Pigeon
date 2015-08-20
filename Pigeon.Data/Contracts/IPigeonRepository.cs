namespace Pigeon.Data.Contracts
{
    using System.Linq;

    public interface IPigeonRepository<T>
        where T : class
    {
        void Create(T entity);

        IQueryable<T> Read();

        void Update(T entity);

        void Delete(T entity);

        void Detach(T entity);
    }
}