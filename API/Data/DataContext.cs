using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DataContext(DbContextOptions<DataContext> options, bool migrate = true) : base(options)
        {
            if (migrate) Database.Migrate();
        }
    }
}
