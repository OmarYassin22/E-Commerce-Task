
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace E_Commerce.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base("DefaultConnection") { }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithMany(o => o.Customers)
                .Map(m =>
                {
                    m.ToTable("CustomersOrders");
                    m.MapLeftKey("CustomerId");
                    m.MapRightKey("OrderId");
                });
        }
        public DbSet<Order> Orders { get; set; }
    }
}