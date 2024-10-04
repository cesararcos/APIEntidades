using APIEntidades.Domain.Dto;

namespace APIEntidades.Application.Contracts
{
    public interface IVideoJuegosAppService
    {
        ResponseDto<IEnumerable<VideoJuegosDto>> Get();
        ResponseDto<VideoJuegosDto> GetById(Guid id);
    }
}
