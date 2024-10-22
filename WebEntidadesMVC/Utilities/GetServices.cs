using WebEntidadesMVC.Models;

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

        public async Task<string> GetGamesAsync(int page, string nombre, string compania, string ano, string token)
        {
            var gameInfo = new
            {
                Page = page,
                Nombre = nombre,
                Compania = compania,
                Ano = ano
            };

            var response = await _httpClient.PostAsJsonAsync("GetGamesPaginate", gameInfo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!.Token!;
            }

            return string.Empty;
        }

    }
}
