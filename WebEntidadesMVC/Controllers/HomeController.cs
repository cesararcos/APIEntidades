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
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            // Obtener el token de autenticación del contexto actual
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Verificamos si el token está disponible
            if (string.IsNullOrEmpty(accessToken))
            {
                return View("Error", "No se encontró un token de acceso válido.");
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
                GetToken getToken = new(); 
                var token = await getToken.GetTokenAsync(ingresoViewModel!.Correo!, ingresoViewModel!.Clave!);

                if (string.IsNullOrEmpty(token))
                    return Unauthorized();

                //// Obtener el token de acceso del contexto de autenticación
                //var accessToken = await HttpContext.GetTokenAsync(ingresoViewModel.Correo!);

                //// Verificamos que el token no sea nulo o vacío
                //if (string.IsNullOrEmpty(accessToken))
                //{
                //    return View("Error", "No se encontró un token de acceso válido.");
                //}

                // Guardar el token como una cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,  // Para que solo sea accesible vía HTTP y no por scripts en el cliente
                    IsEssential = true,
                    //Secure = true,    // Solo enviar la cookie a través de HTTPS
                    SameSite = SameSiteMode.None, // Para proteger contra CSRF (ajústalo según tus necesidades)
                    //Expires = DateTimeOffset.UtcNow.AddHours(1) // Configurar tiempo de expiración de la cookie (1 hora, en este caso)
                };

                // Guardamos el token de acceso como cookie
                HttpContext.Response.Cookies.Append("AccessToken", token, cookieOptions);
                
                // Ahora puedes continuar haciendo la llamada a la API
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("http://localhost:5275/api/Entidades/GetGames");

                if (response.IsSuccessStatusCode)
                {
                    var gestores = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<VideojuegosViewModel>>>();
                    List<VideojuegosViewModel> list = gestores!.Data.Select(x => new VideojuegosViewModel()
                    {
                        Compania = x.Compania
                    }).ToList();
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
            // Recuperar el token de la cookie
            if (HttpContext.Request.Cookies.ContainsKey("AccessToken"))
            {
                // Recupera el valor de la cookie
                string accessTokenFromCookie = HttpContext.Request.Cookies["AccessToken"]!;

            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
