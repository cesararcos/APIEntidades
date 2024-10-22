using Microsoft.AspNetCore.Mvc;
using WebEntidadesMVC.Utilities;

namespace WebEntidadesMVC.Controllers
{
    public class GamesController(HttpClient httpClient) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IActionResult> GetPages()
        {
            GetServices getServices = new();

            if (HttpContext.Session.GetString("AccessToken") == null)
                return NotFound(); // REDIRECCIONAR A VISTA ERROR O LOGIN**

            // Recuperar el string almacenado en la sesión
            var token = HttpContext.Session.GetString("AccessToken");

            //var GameFilter = await getServices.GetGamesAsync();


            return RedirectToAction("Index", "Home");
        }
    }
}
