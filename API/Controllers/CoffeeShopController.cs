using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoffeeShopController : ControllerBase
    {
        private readonly ILogger<CoffeeShopController> _logger;
        private readonly ICoffeeShopService _coffeeShopService;

        public CoffeeShopController(ILogger<CoffeeShopController> logger, ICoffeeShopService coffeeShopService)
        {
            _logger = logger;
            _coffeeShopService = coffeeShopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoffeeShops()
        {
            var coffeeShops = await _coffeeShopService
                .GetCoffeeShopsAsync()
                .ConfigureAwait(false);

            return Ok(coffeeShops);
        }
    }
}