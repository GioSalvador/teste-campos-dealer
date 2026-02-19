namespace TesteCamposDealer.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }
        public int TotalProdutos { get; set; }
        public int TotalVendas { get; set; }
        public decimal ValorTotalVendido { get; set; }

        public List<RankingVendaViewModel> Ranking { get; set; }
    }
}
