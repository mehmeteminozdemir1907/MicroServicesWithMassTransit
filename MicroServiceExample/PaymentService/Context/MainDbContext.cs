using Domain;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Context
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }
    }
}