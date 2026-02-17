using Microsoft.AspNetCore.Mvc;
using TesteCamposDealer.Web.Services;

namespace TesteCamposDealer.Web.Controllers
{
    public class VendaController : Controller
    {
        private readonly VendaService _vendaService;

        public VendaController(VendaService vendaService)
        {
            _vendaService = vendaService;
        }

        public async Task<IActionResult> Index()
        {
            var vendas = await _vendaService.ObterTodasAsync();
            return View(vendas);
        }
    }
}
