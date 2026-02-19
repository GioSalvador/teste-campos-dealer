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
}
