using WebEntidadesMVC.Models;

namespace WebEntidadesMVC.Utilities
{
    public class GetToken
    {
        public async Task<string> GetTokenAsync(string username, string password)
        {
            var client = new HttpClient();
            var loginInfo = new
            {
                Correo = username,
                Clave = password
            };

            var response = await client.PostAsJsonAsync("http://localhost:5275/api/Entidades/Login", loginInfo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!.Token!; // Supongamos que el token está en la propiedad "Token"
            }

            return null;
        }

    }
}
