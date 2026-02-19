using System.ComponentModel.DataAnnotations;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ClienteEditViewModel
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Informe o nome do cliente")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "Informe o endereço")]
        public string Endereco { get; set; }
    }
}
