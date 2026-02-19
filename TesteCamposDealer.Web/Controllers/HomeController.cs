using Microsoft.AspNetCore.Mvc;
using TesteCamposDealer.Web.Services;
using TesteCamposDealer.Web.ViewModels;


namespace TesteCamposDealer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly VendaService _vendaService;
        private readonly ClienteService _clienteService;
        private readonly ProdutoService _produtoService;

        public HomeController(
            VendaService vendaService,
            ClienteService clienteService,
            ProdutoService produtoService)
        {
            _vendaService = vendaService;
            _clienteService = clienteService;
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index()
        {
            var vendas = await _vendaService.ObterTodasAsync();
            var clientes = await _clienteService.ObterTodosAsync();
            var produtos = await _produtoService.ObterTodosAsync();
            var ranking = await _vendaService.ObterRankingAsync();

            var model = new DashboardViewModel
            {
                TotalClientes = clientes.Count,
                TotalProdutos = produtos.Count,
                TotalVendas = vendas.Count,
                ValorTotalVendido = vendas.Sum(v => v.VlrTotalVenda),
                Ranking = ranking
            };

            return View(model);
        }
    }
}

