using Domain;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Context
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Stock>().HasData(
                new Stock { Id = 1, Code = "Stock-123", Name = "Telefon", Quantity = 350 },
                new Stock { Id = 2, Code = "Stock-456", Name = "Bilgisayar", Quantity = 15 },
                new Stock { Id = 3, Code = "Stock-789", Name = "Ütü", Quantity = 2 }
            );
        }
    }
}