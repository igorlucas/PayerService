using API.Models;

namespace API.Services
{
    public interface IServiceAsync<T> where T : class
    {
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<GenericApiResponseEntityList<T>> ListAsync();
        Task<GenericApiResponseEntity<T>> ReadByIdAsync(string id);
    }

    public interface IService<T> where T : class
    {
        int Create(T entity);
        int Update(T entity);
        int Delete(T entity);
        GenericApiResponseEntityList<T> List(); 
        GenericApiResponseEntity<T> ReadById(string id);    
    }
}