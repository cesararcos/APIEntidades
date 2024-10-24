namespace WebEntidadesMVC.Utilities.Contracts
{
    public interface IGetHomeService
    {
        Task<string> GetTokenAsync(string username, string password);
        Task<bool> RegisterUserAsync(string correo, string clave, string usuario);
    }
}
