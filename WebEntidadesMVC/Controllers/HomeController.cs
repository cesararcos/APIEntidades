using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using WebEntidadesMVC.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using WebEntidadesMVC.Utilities;

namespace WebEntidadesMVC.Controllers
{
    public class HomeController(HttpClient httpClient) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IngresoViewModel ingresoViewModel)
        {
            try
            {
                GetServices getServices = new();
                var token = await getServices.GetTokenAsync(ingresoViewModel!.Correo!, ingresoViewModel!.Clave!);

                if (string.IsNullOrEmpty(token))
                    return Unauthorized();

                // Creación Variable session
                HttpContext.Session.SetString("AccessToken", token);

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
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5275/api/Entidades/Register", registerViewModel);

                if (response.IsSuccessStatusCode)
                {
                    var gestores = await response.Content.ReadFromJsonAsync<ResponseDto<bool>>();

                    if (gestores!.Success && gestores!.Data)
                        return RedirectToAction("Index");

                    return View("Privacy");
                    //return RedirectToAction();
                }
                else
                {
                    return View("Error", $"Error en la respuesta del servidor: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error", $"Ocurrió un error en la solicitud: {ex.Message}");
            }
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
