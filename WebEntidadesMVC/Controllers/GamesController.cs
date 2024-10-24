using Microsoft.AspNetCore.Mvc;
using WebEntidadesMVC.Models;
using WebEntidadesMVC.Utilities;
using WebEntidadesMVC.Utilities.Contracts;

namespace WebEntidadesMVC.Controllers
{
    public class GamesController(IGetService getService) : Controller
    {
        private readonly IGetService _getService = getService;

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetPages(int Pagina = 1)
        {
            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");

            var gameFilter = await _getService.GetGamesAsync(Pagina, token!);

            if (gameFilter.Count() == 0)
                return RedirectToAction("Index", "Home");

            int totalPag = gameFilter.FirstOrDefault()?.TotalPaginas ?? 0;
            var modeloPaginacion = new PaginacionViewModel<VideojuegosViewModel>
            {
                Elementos = gameFilter.ToList(),
                PaginaActual = Pagina,
                TotalPaginas = totalPag,
                TamanoPagina = 5
            };

            ViewBag.AccessToken = token;

            return View(modeloPaginacion);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Create(CreateGameViewModel createGameViewModel)
        {
            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");

            var response = await _getService.CreateGamesAsync(createGameViewModel.Nombre!, createGameViewModel.Compania!, createGameViewModel.Ano, createGameViewModel.Precio, token!);

            if (!response)
                return View(createGameViewModel);

            TempData["SuccessMessage"] = "¡El registro fue satisfactorio!";

            return RedirectToAction("GetPages");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetArchiveCsv()
        {
            if (HttpContext.Session.GetString("AccessToken") == null)
                return RedirectToAction("Index", "Home");

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");
            ViewBag.AccessToken = token;

            return View();
        }
    }
}
