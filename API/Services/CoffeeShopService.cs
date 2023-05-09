using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CoffeeShopService : ICoffeeShopService
    {
        private readonly ApplicationDbContext _context;

        public CoffeeShopService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CoffeeShopModel>> GetCoffeeShopsAsync()
        {
            var cofeeShops = await _context.CoffeShops
                .Select(x => new CoffeeShopModel()
                {
                    Address = x.Address,
                    Name = x.Name,
                    Id = x.Id,
                    OpeningHours = x.OpeningHours,
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return cofeeShops;
        }
    }
}
