using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> GetDbSet();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> ReadByIdAsync(Guid id);
        Task<int> CommitAsync();    
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _db;

        public Repository(DataContext db) => _db = db;

        public DbSet<T> GetDbSet() => _db.Set<T>();
        
        public void Create(T entity) => _db.Set<T>().Add(entity);
        public void Delete(T entity) => _db.Set<T>().Remove(entity);
        public void Update(T entity) => _db.Set<T>().Update(entity);

        public async Task<IEnumerable<T>> ReadAllAsync() => await _db.Set<T>().ToArrayAsync();
        public async Task<T> ReadByIdAsync(Guid id) => await _db.Set<T>().FindAsync(id);
        
        public async Task<int> CommitAsync() => await _db.SaveChangesAsync();
    }
}