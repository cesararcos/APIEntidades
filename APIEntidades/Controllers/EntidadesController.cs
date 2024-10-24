using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIEntidades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntidadesController(IUsuarioAppService usuarioAppService, IVideoJuegosAppService videoJuegosAppService) : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioAppService = usuarioAppService;
        private readonly IVideoJuegosAppService _videoJuegosAppService = videoJuegosAppService;

        [HttpPost]
        [Route(nameof(EntidadesController.Register))]
        public IActionResult Register(UsuarioDto usuario)
        {
            var responde = _usuarioAppService.Register(usuario);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }

        [HttpPost]
        [Route(nameof(EntidadesController.Login))]
        public IActionResult Login(IngresoDto ingreso)
        {
            var responde = _usuarioAppService.Login(ingreso);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(new { Token = responde.Data });
        }

        [HttpGet]
        [Authorize]
        [Route(nameof(EntidadesController.GetGames))]
        public IActionResult GetGames()
        {
            var responde = _videoJuegosAppService.Get();

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(EntidadesController.GetGamesById))]
        public IActionResult GetGamesById(FilterGetGamesById filterGetGamesById)
        {
            var responde = _videoJuegosAppService.GetById(filterGetGamesById.Id);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);
            
            return Ok(new
            {
                Nombre = responde.Data.Nombre,
                Compania = responde.Data.Compania,
                Ano = responde.Data.Ano,
                Precio = responde.Data.Precio
            });
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(EntidadesController.GetGamesPaginate))]
        public IActionResult GetGamesPaginate(FiltroVideoJuegoDto filtroVideoJuegoDto)
        {
            var responde = _videoJuegosAppService.GetGamesPaginate(filtroVideoJuegoDto);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(SaveVideoGame))]
        public IActionResult SaveVideoGame(VideoJuegosDto videoJuegosDto)
        {
            var responde = _videoJuegosAppService.SaveVideoGame(videoJuegosDto);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }

        [HttpPut]
        [Authorize]
        [Route("{id:Guid}/" + nameof(EditVideoGame))]
        public IActionResult EditVideoGame(Guid id, VideoJuegosDto videoJuegosDto)
        {
            var responde = _videoJuegosAppService.EditVideoGame(id, videoJuegosDto);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:Guid}/" + nameof(DeleteVideoGame))]
        public IActionResult DeleteVideoGame(Guid id)
        {
            var responde = _videoJuegosAppService.DeleteVideoGame(id);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(GetArchiveCsv))]
        public IActionResult GetArchiveCsv(FilterGetArchiveDto filterGetArchiveDto)
        {
            var responde = _videoJuegosAppService.GetArchiveCsv(filterGetArchiveDto.Top);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return File(responde.Data, "text/csv", "ranking_videojuegos.csv");
        }

        [HttpGet]
        [Authorize]
        [Route(nameof(GetProcedure))]
        public IActionResult GetProcedure([FromQuery] int cantidad)
        {
            var responde = _videoJuegosAppService.GetProcedure(cantidad);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
        }
    }
}
