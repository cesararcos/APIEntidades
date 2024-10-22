using Microsoft.AspNetCore.Mvc;

namespace WebEntidadesMVC.Controllers
{
    public class GamesController(HttpClient httpClient) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;

        public IActionResult GetPages()
        {
            // Creación Variable session
            HttpContext.Session.SetString("Usuario", "Juan Pérez");

            
            return RedirectToAction("Index", "Home");
        }
    }
}
