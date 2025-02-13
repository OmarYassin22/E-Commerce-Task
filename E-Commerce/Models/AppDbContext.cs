
using System.Data.Entity;

namespace E_Commerce.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(): base("DefaultConnection")
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}