using System.ComponentModel.DataAnnotations;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ClienteCreateViewModel
    {
        [Required(ErrorMessage = "Informe o nome do cliente")]
        public string nomeCliente { get; set; }

        [Required(ErrorMessage = "Informe o endereço")]
        public string endereco { get; set; }
    }
}
