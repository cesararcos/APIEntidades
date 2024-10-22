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

        public async Task<IActionResult> Login()
        {
            // Obtener el token de autenticaci�n del contexto actual
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Verificamos si el token est� disponible
            if (string.IsNullOrEmpty(accessToken))
            {
                return View("Error", "No se encontr� un token de acceso v�lido.");
            }

            // Agregar el token al encabezado Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("http://localhost:5275/api/Entidades/GetGames");
            if (response.IsSuccessStatusCode)
            {
                var gestores = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<VideojuegosViewModel>>>();
                List<VideojuegosViewModel> list = gestores!.Data.Select(x => new VideojuegosViewModel()
                {
                    Compania = x.Compania
                }).ToList();
                return View(list);
            }

            return View("Error");
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

                // Creaci�n Variable session
                HttpContext.Session.SetString("AccessToken", token);

                return RedirectToAction("GetPages", "Games");

                var response = await _httpClient.GetAsync("http://localhost:5275/api/Entidades/GetGames");

                if (response.IsSuccessStatusCode)
                {
                    var gestores = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<VideojuegosViewModel>>>();
                    List<VideojuegosViewModel> list = gestores!.Data.Select(x => new VideojuegosViewModel()
                    {
                        Compania = x.Compania
                    }).ToList();
                    //return View("Privacy");
                    return RedirectToAction("GetPages", "Games");
                }
                else
                {
                    return View("Error", $"Error en la respuesta del servidor: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error", $"Ocurri� un error en la solicitud: {ex.Message}");
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
                return View("Error", $"Ocurri� un error en la solicitud: {ex.Message}");
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
