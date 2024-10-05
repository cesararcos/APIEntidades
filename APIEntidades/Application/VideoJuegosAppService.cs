﻿using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using APIEntidades.Domain.Entities;
using APIEntidades.Infrastructure.DataAccess;
using APIEntidades.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace APIEntidades.Application
{
    public class VideoJuegosAppService(EntidadesDbContext context, IMemoryCache memoryCache) : IVideoJuegosAppService
    {
        private readonly EntidadesDbContext _context = context;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly string cacheKey = "VideoGameList"; // Clave para el cache

        public ResponseDto<IEnumerable<VideoJuegosDto>> Get()
        {
            try
            {
                if (!_memoryCache.TryGetValue(cacheKey, out List<Videojuegos>? videoGames))
                {
                    videoGames = _context.Videojuegos.ToList();

                    if (videoGames.Count == 0)
                    {
                        return new ResponseDto<IEnumerable<VideoJuegosDto>>
                        {
                            Success = false,
                            ErrorMessage = Constants.NOT_EXIST
                        };
                    }

                    // Configurar opciones de caché (duración, prioridad, etc.)
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10)) // Caché expira si no se accede en 10 minutos
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))  // Caché expira absolutamente en 1 hora
                        .SetPriority(CacheItemPriority.Normal);         // Prioridad de eliminación

                    // Guardar los datos en caché
                    _memoryCache.Set(cacheKey, videoGames, cacheEntryOptions);
                }

                IEnumerable<VideoJuegosDto> gamesList = videoGames!.Select(u => new VideoJuegosDto()
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
                if (filtroVideoJuegoDto.Page <= 0)
                    filtroVideoJuegoDto.Page = 1;

                int pageSize = 5;
                var gamesFilter = _context?.Videojuegos?.Where(x => x.Nombre == filtroVideoJuegoDto.Nombre || x.Compania == filtroVideoJuegoDto.Compania || x.Ano == filtroVideoJuegoDto.Ano).Skip((filtroVideoJuegoDto.Page - 1) * pageSize).Take(pageSize).ToList() ?? null;

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

        public ResponseDto<bool> SaveVideoGame(VideoJuegosDto videoJuegosDto)
        {
            try
            {
                //var result = _usuarioValidator.Validate(usuario);

                //if (!result.IsValid)
                //{
                //    return new ResponseDto<bool>
                //    {
                //        Success = false,
                //        ErrorMessage = result.Errors.First().ErrorMessage
                //    };
                //}

                var findGame = _context?.Videojuegos?.FirstOrDefault(x => x.Nombre == videoJuegosDto.Nombre) ?? null;

                if (findGame != null)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = Constants.GAME_EXIST
                    };
                }

                Videojuegos videojuegos = new()
                {
                    Id = Guid.NewGuid(),
                    Nombre = videoJuegosDto.Nombre,
                    Compania = videoJuegosDto.Compania,
                    Ano = videoJuegosDto.Ano,
                    Precio = videoJuegosDto.Precio,
                    Puntaje = videoJuegosDto.Puntaje
                };

                _context!.Videojuegos.Add(videojuegos);
                _context!.SaveChanges();


                return new ResponseDto<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }

        public ResponseDto<bool> EditVideoGame(Guid id, VideoJuegosDto videoJuegosDto)
        {
            try
            {
                //var result = _usuarioValidator.Validate(usuario);

                //if (!result.IsValid)
                //{
                //    return new ResponseDto<bool>
                //    {
                //        Success = false,
                //        ErrorMessage = result.Errors.First().ErrorMessage
                //    };
                //}

                Videojuegos? findGame = _context?.Videojuegos?.FirstOrDefault(x => x.Id == id) ?? null;

                if (findGame == null)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                findGame.Nombre = videoJuegosDto.Nombre;
                findGame.Compania = videoJuegosDto.Compania;
                findGame.Ano = videoJuegosDto.Ano;
                findGame.Precio = videoJuegosDto.Precio;
                findGame.Puntaje = videoJuegosDto.Puntaje;

                _context!.SaveChanges();

                return new ResponseDto<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }

        public ResponseDto<bool> DeleteVideoGame(Guid id)
        {
            try
            {
                //var result = _usuarioValidator.Validate(usuario);

                //if (!result.IsValid)
                //{
                //    return new ResponseDto<bool>
                //    {
                //        Success = false,
                //        ErrorMessage = result.Errors.First().ErrorMessage
                //    };
                //}

                Videojuegos? findGame = _context?.Videojuegos?.FirstOrDefault(x => x.Id == id) ?? null;

                if (findGame == null)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                // Eliminar de la base de datos
                _context!.Videojuegos.Remove(findGame);
                _context!.SaveChanges();

                return new ResponseDto<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
