using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using APIEntidades.Domain.Entities;
using APIEntidades.Infrastructure.DataAccess;
using APIEntidades.Infrastructure.Helpers;
using APIEntidades.Utilities.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace APIEntidades.Application
{
    public class VideoJuegosAppService(EntidadesDbContext context, IValidator<int> archiveValidator, IValidator<VideoJuegosDto> gameValidator, IMemoryCache memoryCache) : IVideoJuegosAppService
    {
        private readonly EntidadesDbContext _context = context;
        private readonly IValidator<int> _archiveValidator = archiveValidator;
        private readonly IValidator<VideoJuegosDto> _gameValidator = gameValidator;
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
                var consulta = _context.Videojuegos.AsQueryable();

                if (!string.IsNullOrEmpty(filtroVideoJuegoDto.Nombre))
                    consulta = consulta.Where(x => x.Nombre == filtroVideoJuegoDto.Nombre);
                if (!string.IsNullOrEmpty(filtroVideoJuegoDto.Compania))
                    consulta = consulta.Where(x => x.Compania == filtroVideoJuegoDto.Compania);
                if (filtroVideoJuegoDto.Ano > 0)
                    consulta = consulta.Where(x => x.Ano == filtroVideoJuegoDto.Ano);

                // Total de registros después de aplicar los filtros
                var totalRegistros = consulta.Count();

                consulta = consulta.Skip((filtroVideoJuegoDto.Page - 1) * pageSize).Take(pageSize);
                
                // Calcular el total de páginas
                int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);

                IEnumerable<VideoJuegosDto> gamesList = consulta.Select(u => new VideoJuegosDto()
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Compania = u.Compania,
                    Ano = u.Ano,
                    Precio = u.Precio,
                    Puntaje = u.Puntaje,
                    FechaActualizacion = u.FechaActualizacion,
                    Usuario = u.Usuario,
                    TotalPaginas = totalPaginas
                }).ToList();

                if (gamesList.Count() == 0)
                {
                    return new ResponseDto<IEnumerable<VideoJuegosDto>>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                return new ResponseDto<IEnumerable<VideoJuegosDto>>
                {
                    Success = true,
                    Data = gamesList.ToList()
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
                var result = _gameValidator.Validate(videoJuegosDto);

                if (!result.IsValid)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = result.Errors.First().ErrorMessage
                    };
                }

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
                if (id == Guid.Empty)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = Constants.EMPTY
                    };
                }

                var result = _gameValidator.Validate(videoJuegosDto);

                if (!result.IsValid)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = result.Errors.First().ErrorMessage
                    };
                }

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
                findGame.FechaActualizacion = DateTime.Now;
                findGame.Usuario = videoJuegosDto.Usuario;

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
                if (id == Guid.Empty)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = Constants.EMPTY
                    };
                }

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

        public ResponseDto<MemoryStream> GetArchiveCsv(int? top)
        {
            try
            {
                FluentValidation.Results.ValidationResult result = new();

                if (top.HasValue)
                    result = _archiveValidator.Validate(top!.Value);

                if (!result.IsValid)
                {
                    return new ResponseDto<MemoryStream>
                    {
                        Success = false,
                        ErrorMessage = result.Errors.First().ErrorMessage
                    };
                }

                IEnumerable<Videojuegos> videoGames = _context.Videojuegos.Include(x => x.Calificaciones).ToList();

                if (!videoGames.Any())
                {
                    return new ResponseDto<MemoryStream>
                    {
                        Success = false,
                        ErrorMessage = Constants.NOT_EXIST
                    };
                }

                int filter = top ?? videoGames.Count();
                List<GameRankingDto> rankingList = videoGames.Select(v => new GameRankingDto()
                {
                    Title = v.Nombre,
                    Company = v.Compania,
                    Score = v.Calificaciones.Any() ? v.Calificaciones.Average(r => r.Puntaje ?? 0.0m) : 0
                }).OrderByDescending(v => v.Score).Take(filter).ToList();

                // Clasificación
                int media = (filter) / 2;

                for (int i = 0; i < rankingList.Count; i++)
                {
                    rankingList[i].Classification = i + 1 <= media ? "GOTY" : "AAA";
                }

                var csvData = new StringBuilder();
                csvData.AppendLine("Titulo|Compania|Puntaje|Clasificacion");

                foreach (var item in rankingList)
                {
                    csvData.AppendLine($"{item.Title}|{item.Company}|{item.Score}|{item.Classification}");
                }

                var byteArray = Encoding.UTF8.GetBytes(csvData.ToString());
                var stream = new MemoryStream(byteArray);

                return new ResponseDto<MemoryStream>
                {
                    Success = true,
                    Data = stream
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<MemoryStream>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }

        public ResponseDto<ProcedureDto> GetProcedure(int cantidad)
        {
            try
            {
                SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand command = conn.CreateCommand();
                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GenerarCalificacionesAleatorias";
                command.Parameters.Add("@Cantidad", SqlDbType.Int).Value = cantidad;
                // Definir el parámetro OUTPUT
                SqlParameter codigoDeErrorParam = new()
                {
                    ParameterName = "@CodigoDeError",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output  // Definir como parámetro OUTPUT
                };
                command.Parameters.Add(codigoDeErrorParam);
                SqlParameter mensajeDeErrorParam = new()
                {
                    ParameterName = "@MensajeDeError",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Direction = ParameterDirection.Output  // Definir como parámetro OUTPUT
                };
                command.Parameters.Add(mensajeDeErrorParam);
                command.ExecuteNonQuery();
                ProcedureDto procedure = new()
                {
                    CodigoDeError = (int)command.Parameters["@CodigoDeError"].Value,
                    MensajeDeError = command.Parameters["@MensajeDeError"].Value != DBNull.Value ? (string)command.Parameters["@MensajeDeError"].Value : string.Empty
                };
                conn.Close();

                return new ResponseDto<ProcedureDto>
                {
                    Success = true,
                    Data = procedure
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<ProcedureDto>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
