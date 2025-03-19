using Domain;
using Microsoft.EntityFrameworkCore;

namespace StockService.Context
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        public DbSet<Stock> Stocks { get; set; }
    }
}