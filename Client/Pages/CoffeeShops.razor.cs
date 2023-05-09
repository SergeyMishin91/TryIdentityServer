using API.Models;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;

namespace Client.Pages
{
    public partial class CoffeeShops
    {
        private List<CoffeeShopModel> Shops = new();

        [Inject] private HttpClient HttpClient { get; set; }

        [Inject] private IConfiguration Config { get; set; }
        [Inject] private ITokenService TokenService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tokenResponse = await TokenService.GetToken(
                "CoffeeAPI.read");
            HttpClient.SetBearerToken(tokenResponse.AccessToken);

            HttpClient.BaseAddress = new Uri("https://localhost:5445");
            //HttpClient.BaseAddress = new Uri($"{Config["apiUri"]}");

            var result = await HttpClient.GetAsync("/api/coffeeshop");
            //Config["apiUri"] + "/api/coffeeshop");

            if (result.IsSuccessStatusCode)
            {
                Shops = await result.Content.ReadFromJsonAsync
                    <List<CoffeeShopModel>>();
            }
            else
            {
                string errMsg = await result.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(errMsg);
                throw new Exception(errMsg);
            }
        }
    }
}
