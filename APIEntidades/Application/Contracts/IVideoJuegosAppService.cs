using APIEntidades.Domain.Dto;
using System.Text;

namespace APIEntidades.Application.Contracts
{
    public interface IVideoJuegosAppService
    {
        ResponseDto<IEnumerable<VideoJuegosDto>> Get();
        ResponseDto<VideoJuegosDto> GetById(Guid id);
        ResponseDto<IEnumerable<VideoJuegosDto>> GetGamesPaginate(FiltroVideoJuegoDto filtroVideoJuegoDto);
        ResponseDto<bool> SaveVideoGame(VideoJuegosDto videoJuegosDto);
        ResponseDto<bool> EditVideoGame(Guid id, VideoJuegosDto videoJuegosDto);
        ResponseDto<bool> DeleteVideoGame(Guid id);
        ResponseDto<MemoryStream> GetArchiveCsv(int? top);
    }
}
