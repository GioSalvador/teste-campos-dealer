using System.Net.Http.Json;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Web.Services
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }
        public async Task<List<ClienteViewModel>> ObterTodosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ClienteViewModel>>("cliente");
        }
        public async Task<ClienteViewModel> ObterPorIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ClienteViewModel>($"cliente/{id}");
        }
        public async Task CriarAsync(ClienteCreateViewModel cliente)
        {
            var response = await _httpClient.PostAsJsonAsync("cliente", cliente);

            response.EnsureSuccessStatusCode();
        }
        public async Task AtualizarAsync(int id, ClienteEditViewModel cliente)
        {
            var response = await _httpClient.PutAsJsonAsync($"cliente/{id}", cliente);

            response.EnsureSuccessStatusCode();
        }
        public async Task DeletarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"cliente/{id}");

            response.EnsureSuccessStatusCode();
        }
    }

}
