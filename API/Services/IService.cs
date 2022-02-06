using API.Models;

namespace API.Services
{
    public interface IService<T> where T : class
    {
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);    
        Task<GenericApiResponseEntityList<T>> ReadAllAsync();
        Task<GenericApiResponseEntity<T>> ReadByIdAsync(Guid id);
    }
}