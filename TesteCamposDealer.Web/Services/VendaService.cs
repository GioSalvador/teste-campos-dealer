using System.Net.Http.Json;
using TesteCamposDealer.Web.ViewModels;
namespace TesteCamposDealer.Web.Services
{
    public class VendaService
    {
        private readonly HttpClient _httpClient;

        public VendaService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<List<VendaViewModel>> ObterTodasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<VendaViewModel>>("venda");
        }

        public async Task CriarAsync(VendaCreateViewModel venda)
        {
            var response = await _httpClient.PostAsJsonAsync("venda", venda);
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<RankingVendaViewModel>> ObterRankingAsync()
        {
            return await _httpClient
                .GetFromJsonAsync<List<RankingVendaViewModel>>("venda/ranking");
        }

    }
}
