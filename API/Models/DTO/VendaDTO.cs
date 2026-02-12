using System.Collections.Generic;

namespace TesteCamposDealer.Models.DTO

{
    public class VendaDTO
    {
        public int idCliente { get; set; }
        public List<VendaItemDTO> itens { get; set; }
    }
}


