namespace ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
