namespace AFSTranslator.Interfaces.Repository
{
    public interface IRepository<T>
    {
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<bool> AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
    }
}