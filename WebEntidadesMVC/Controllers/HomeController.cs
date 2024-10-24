using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebEntidadesMVC.Models;
using WebEntidadesMVC.Utilities.Contracts;

namespace WebEntidadesMVC.Controllers
{
    public class HomeController(HttpClient httpClient, IGetHomeService getHomeService) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IGetHomeService _getHomeService = getHomeService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IngresoViewModel ingresoViewModel)
        {
            try
            {
                var token = await _getHomeService.GetTokenAsync(ingresoViewModel!.Correo!, ingresoViewModel!.Clave!);

                if (string.IsNullOrEmpty(token))
                    return Unauthorized();

                // Creación Variable session
                HttpContext.Session.SetString("AccessToken", token);
                HttpContext.Session.SetString("EmailAccess", ingresoViewModel.Correo!);

                return RedirectToAction("GetPages", "Games");
            }
            catch (HttpRequestException ex)
            {
                return View("Error", $"Ocurrió un error en la solicitud: {ex.Message}");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            try
            {
                var response = await _getHomeService.RegisterUserAsync(registerViewModel.Correo ?? string.Empty, registerViewModel.Clave ?? string.Empty, registerViewModel.Usuario ?? string.Empty);

                if (!response)
                    return View(registerViewModel);

                TempData["SuccessMessage"] = "¡El registro fue satisfactorio!";

                return RedirectToAction("Register");
            }
            catch (HttpRequestException ex)
            {
                return View("Error", $"Ocurrió un error en la solicitud: {ex.Message}");
            }
        }

        public IActionResult Logout()
        {
            // Eliminar sesiones
            HttpContext.Session.Remove("AccessToken");
            HttpContext.Session.Remove("EmailAccess");

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
