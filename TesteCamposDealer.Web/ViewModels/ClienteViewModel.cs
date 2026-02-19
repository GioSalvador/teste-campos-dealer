using System;
using System.Collections.Generic;

namespace TesteCamposDealer.Web.ViewModels
{
    public class ClienteViewModel
    {
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Endereco { get; set; }
        public DateTime DthRegistro { get; set; }

        public List<VendaResumoViewModel> Vendas { get; set; }
    }

    public class VendaResumoViewModel
    {
        public int IdVenda { get; set; }
        public DateTime DthRegistro { get; set; }
        public decimal VlrTotalVenda { get; set; }

        public List<ItemVendaResumoViewModel> VendaItems { get; set; }
    }

    public class ItemVendaResumoViewModel
    {
        public int IdVendaItem { get; set; }
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal VlrUnitario { get; set; }
        public decimal VlrTotalItem { get; set; }
    }
}
