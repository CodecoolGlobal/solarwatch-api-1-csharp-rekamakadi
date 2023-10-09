namespace SolarWatch.Services.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(InClassName inClassName);
    Task<IEnumerable<T>> GetAllAsync();
    void AddAsync(T entity);
    void DeleteAsync(int id);
    void UpdateAsync(T entity);
}