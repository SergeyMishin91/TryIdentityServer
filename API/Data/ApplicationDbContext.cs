using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<CoffeeShop> CoffeShops { get; set; }
    }
}
