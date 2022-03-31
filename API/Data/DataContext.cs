using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options, bool migrate = true) : base(options)
        {
            if (migrate) Database.Migrate();
        }
    }
}