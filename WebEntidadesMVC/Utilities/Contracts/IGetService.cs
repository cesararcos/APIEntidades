using WebEntidadesMVC.Models;

namespace WebEntidadesMVC.Utilities.Contracts
{
    public interface IGetService
    {
        Task<IEnumerable<VideojuegosViewModel>> GetGamesAsync(int page, string token);
        Task<bool> CreateGamesAsync(string nombre, string compania, int? ano, decimal? precio, string token);
    }
}
