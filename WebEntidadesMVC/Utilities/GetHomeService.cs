using WebEntidadesMVC.Models;
using WebEntidadesMVC.Utilities.Contracts;

namespace WebEntidadesMVC.Utilities
{
    public class GetHomeService : IGetHomeService
    {
        private readonly HttpClient _httpClient;

        public GetHomeService()
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

        public async Task<bool> RegisterUserAsync(string correo, string clave, string usuario)
        {
            var userInfo = new
            {
                Correo = correo,
                Clave = clave,
                Usuario = usuario
            };

            var response = await _httpClient.PostAsJsonAsync("Register", userInfo);

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
