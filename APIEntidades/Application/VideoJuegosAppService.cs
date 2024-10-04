using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using APIEntidades.Infrastructure.DataAccess;
using APIEntidades.Infrastructure.Helpers;
using APIEntidades.Utilities.Validators;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace APIEntidades.Application
{
    public class VideoJuegosAppService : IVideoJuegosAppService
    {
        private readonly EntidadesDbContext _context;
        public VideoJuegosAppService(EntidadesDbContext context)
        {
            _context = context;
        }

        public ResponseDto<IEnumerable<VideoJuegosDto>> Get()
        {
            try
            {
                var games = _context?.Videojuegos?.ToList() ?? null;

                if (games == null)
                {
                    return new ResponseDto<IEnumerable<VideoJuegosDto>>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                IEnumerable<VideoJuegosDto> gamesList = games.Select(u => new VideoJuegosDto()
                {
                    Nombre = u.Nombre,
                    Compania = u.Compania,
                    Ano = u.Ano,
                    Precio = u.Precio,
                    Puntaje = u.Puntaje,
                    FechaActualizacion = u.FechaActualizacion,
                    Usuario = u.Usuario,
                }).ToList();

                return new ResponseDto<IEnumerable<VideoJuegosDto>>
                {
                    Success = true,
                    Data = gamesList
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<VideoJuegosDto>>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }

        public ResponseDto<VideoJuegosDto> GetById(Guid id)
        {
            try
            {
                var game = _context?.Videojuegos?.FirstOrDefault(x => x.Id == id) ?? null;

                if (game == null)
                {
                    return new ResponseDto<VideoJuegosDto>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                VideoJuegosDto response = new()
                {
                    Nombre = game.Nombre,
                    Compania = game.Compania,
                    Ano = game.Ano,
                    Precio = game.Precio,
                    Puntaje = game.Puntaje,
                    FechaActualizacion = game.FechaActualizacion,
                    Usuario = game.Usuario
                };

                return new ResponseDto<VideoJuegosDto>
                {
                    Success = true,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<VideoJuegosDto>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }

        public ResponseDto<IEnumerable<VideoJuegosDto>> GetGamesPaginate(FiltroVideoJuegoDto filtroVideoJuegoDto)
        {
            try
            {
                if (filtroVideoJuegoDto.page <= 0)
                    filtroVideoJuegoDto.page = 1;

                int pageSize = 5;
                var gamesFilter = _context?.Videojuegos?.Where(x => x.Nombre == filtroVideoJuegoDto.Nombre && x.Compania == filtroVideoJuegoDto.Compania && x.Ano == filtroVideoJuegoDto.Ano).Skip((filtroVideoJuegoDto.page - 1) * pageSize).Take(pageSize).ToList() ?? null;

                if (gamesFilter == null)
                {
                    return new ResponseDto<IEnumerable<VideoJuegosDto>>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                IEnumerable<VideoJuegosDto> gamesList = gamesFilter.Select(u => new VideoJuegosDto()
                {
                    Nombre = u.Nombre,
                    Compania = u.Compania,
                    Ano = u.Ano,
                    Precio = u.Precio,
                    Puntaje = u.Puntaje,
                    FechaActualizacion = u.FechaActualizacion,
                    Usuario = u.Usuario,
                }).ToList();

                return new ResponseDto<IEnumerable<VideoJuegosDto>>
                {
                    Success = true,
                    Data = gamesList
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<VideoJuegosDto>>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
