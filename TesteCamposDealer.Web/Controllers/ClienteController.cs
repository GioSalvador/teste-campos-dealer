using Microsoft.AspNetCore.Mvc;
using TesteCamposDealer.Web.Services;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObterTodosAsync();

            clientes = clientes
            .OrderByDescending(c => c.DthRegistro)
            .ToList();
            return View(clientes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteCreateViewModel cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteService.CriarAsync(cliente);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            var model = new ClienteEditViewModel
            {
                IdCliente = cliente.IdCliente,
                NomeCliente = cliente.NomeCliente,
                Endereco = cliente.Endereco
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteEditViewModel cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteService.AtualizarAsync(id, cliente);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteService.DeletarAsync(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
