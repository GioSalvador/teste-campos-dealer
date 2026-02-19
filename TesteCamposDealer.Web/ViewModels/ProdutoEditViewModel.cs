using System.ComponentModel.DataAnnotations;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ProdutoEditViewModel
    {
        public int idProduto { get; set; }

        [Required(ErrorMessage = "Informe a descrição")]
        public string dscProduto { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal preco { get; set; }
    }
}
