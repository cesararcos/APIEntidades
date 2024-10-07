using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIEntidades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntidadesController : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly IVideoJuegosAppService _videoJuegosAppService;

        public EntidadesController(IUsuarioAppService usuarioAppService, IVideoJuegosAppService videoJuegosAppService)
        {
            _usuarioAppService = usuarioAppService;
            _videoJuegosAppService = videoJuegosAppService;
        }

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
        public IActionResult GetGamesById([FromBody] Guid id)
        {
            var responde = _videoJuegosAppService.GetById(id);

            if (!responde.Success)
                return NotFound(responde.ErrorMessage);

            return Ok(responde);
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

        [HttpGet]
        [Authorize]
        [Route(nameof(GetArchiveCsv))]
        public IActionResult GetArchiveCsv([FromQuery] int? top)
        {
            var responde = _videoJuegosAppService.GetArchiveCsv(top);

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
