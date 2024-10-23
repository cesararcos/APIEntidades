using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebEntidadesMVC.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebEntidadesMVC.Utilities
{
    public class GetServices
    {
        private readonly HttpClient _httpClient;

        public GetServices()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5275/api/Entidades/");
        }
        public async Task<string> GetTokenAsync(string username, string password)
        {
            var loginInfo = new
            {
                Correo = username,
                Clave = password
            };

            var response = await _httpClient.PostAsJsonAsync("Login", loginInfo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!.Token!;
            }

            return string.Empty;
        }

        public async Task<IEnumerable<VideojuegosViewModel>> GetGamesAsync(int page, string token)
        {
            List<VideojuegosViewModel> videojuegosViewModels = new();

            var gameInfo = new
            {
                Page = page,
                Nombre = string.Empty,
                Compania = string.Empty,
                Ano = 0
            };

            // Autenticación y llamada a la API
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("GetGamesPaginate", gameInfo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<VideojuegosViewModel>>>();

                if (result!.Success)
                {
                    videojuegosViewModels = result.Data.Select(x => new VideojuegosViewModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Ano = x.Ano,
                        Compania = x.Compania,
                        FechaActualizacion = x.FechaActualizacion,
                        Precio = x.Precio,
                        Puntaje = x.Puntaje,
                        Usuario = x.Usuario,
                        TotalPaginas = x.TotalPaginas
                    }).ToList();
                }
            }

            return videojuegosViewModels;
        }

        public async Task<bool> CreateGamesAsync(string nombre, string compania, int? ano, decimal? precio, decimal? puntaje, string token)
        {
            var gameInfo = new
            {
                Nombre = nombre,
                Compania = compania,
                Ano = ano,
                Precio = precio,
                Puntaje = puntaje
            };

            // Autenticación y llamada a la API
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("SaveVideoGame", gameInfo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseDto<bool>>();

                if (result!.Success && result!.Data)
                    return result.Data;
                
            }

            return false;
        }

    }
}
