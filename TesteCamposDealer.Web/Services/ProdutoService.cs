using TesteCamposDealer.Web.ViewModels;
using System.Net.Http.Json;

public class ProdutoService
{
    private readonly HttpClient _httpClient;

    public ProdutoService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("ApiClient");
    }

    public async Task<List<ProdutoViewModel>> ObterTodosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProdutoViewModel>>("produto");
    }

    public async Task<ProdutoViewModel> ObterPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProdutoViewModel>($"produto/{id}");
    }

    public async Task CriarAsync(object produto)
    {
        var response = await _httpClient.PostAsJsonAsync("produto", produto);
        response.EnsureSuccessStatusCode();
    }

    public async Task AtualizarAsync(int id, object produto)
    {
        var response = await _httpClient.PutAsJsonAsync($"produto/{id}", produto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletarAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"produto/{id}");
        response.EnsureSuccessStatusCode();
    }
}
