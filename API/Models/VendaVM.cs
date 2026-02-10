using System;
using System.Collections.Generic;

namespace TesteCamposDealer.Models
{
    public class VendaVM
    {
        public int idCliente {  get; set; }
        public List<ItemVendaVM> Itens { get; set; }
    }

    public class ItemVendaVM
    {
        public int idProduto { get; set; }
        public int quantidade { get; set; }
        public double vlrUnitario { get; set; }
    }
}