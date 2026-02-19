using Microsoft.AspNetCore.Mvc;
using TesteCamposDealer.Web.Services;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Web.Controllers
{
    public class VendaController : Controller
    {
        private readonly VendaService _vendaService;
        private readonly ClienteService _clienteService;
        private readonly ProdutoService _produtoService;

        public VendaController(
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

            var model = vendas
                .OrderByDescending(v => v.DthRegistro)
                .ToList();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new VendaCreateViewModel();

            model.Clientes = await _clienteService.ObterTodosAsync();
            model.Produtos = await _produtoService.ObterTodosAsync();

            model.Itens.Add(new ItemVendaCreateViewModel());

            if (model.IdCliente == 0)
            {
                ModelState.AddModelError("IdCliente", "Selecione um cliente");
            }

            if (model.Itens == null || !model.Itens.Any())
            {
                ModelState.AddModelError("", "Adicione pelo menos um item à venda.");
            }

            if (model.Itens.Any(i => i.Quantidade <= 0))
            {
                ModelState.AddModelError("", "Quantidade inválida em um dos itens.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VendaCreateViewModel venda)
        {
            if (!ModelState.IsValid)
            {
                venda.Clientes = await _clienteService.ObterTodosAsync();
                venda.Produtos = await _produtoService.ObterTodosAsync();
                return View(venda);
            }

            await _vendaService.CriarAsync(venda);

            return RedirectToAction(nameof(Index));
        }
    }
}
