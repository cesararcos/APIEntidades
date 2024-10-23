using Microsoft.AspNetCore.Mvc;
using WebEntidadesMVC.Models;
using WebEntidadesMVC.Utilities;

namespace WebEntidadesMVC.Controllers
{
    public class GamesController() : Controller
    {
        public async Task<IActionResult> GetPages(int Pagina = 1)
        {
            GetServices getServices = new();

            if (HttpContext.Session.GetString("AccessToken") == null)
                return NotFound(); // REDIRECCIONAR A VISTA ERROR O LOGIN**

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");

            var gameFilter = await getServices.GetGamesAsync(Pagina, token!);

            if (gameFilter.Count() == 0)
                return NotFound(); // REDIRECCIONAR A VISTA ERROR O LOGIN**

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
    }
}
