using System.Text.Json.Serialization;

public class ProdutoViewModel
{
    [JsonPropertyName("idProduto")]
    public int IdProduto { get; set; }

    [JsonPropertyName("dscProduto")]
    public string Descricao { get; set; }

    [JsonPropertyName("precoAtual")]
    public decimal Valor { get; set; }
}
