using System.Net.Http.Json;
using TesteCamposDealer.Web.ViewModels;
namespace TesteCamposDealer.Web.Services
{
    public class VendaService
    {
        private readonly HttpClient _httpClient;

        public VendaService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IEnumerable<VendaViewModel>> ObterTodasAsync()
        {
            var response = await _httpClient.GetAsync("venda");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<VendaViewModel>>();
        }

        public async Task<VendaViewModel> ObterPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"venda/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<VendaViewModel>();
        }

        public async Task CriarAsync(VendaViewModel venda)
        {
            var response = await _httpClient.PostAsJsonAsync("venda", venda);

            response.EnsureSuccessStatusCode();
        }

        public async Task AtualizarAsync(int id, VendaViewModel venda)
        {
            var response = await _httpClient.PutAsJsonAsync($"venda/{id}", venda);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeletarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"venda/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
