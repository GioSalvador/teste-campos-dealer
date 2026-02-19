using System.ComponentModel.DataAnnotations;

namespace TesteCamposDealer.Web.ViewModels
{
    public class VendaCreateViewModel
    {
        [Required(ErrorMessage = "Selecione um cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente")]
        public int IdCliente { get; set; }
        public List<ItemVendaCreateViewModel> Itens { get; set; } = new();

        public List<ClienteViewModel> Clientes { get; set; } = new();
        public List<ProdutoViewModel> Produtos { get; set; } = new();
    }

}
