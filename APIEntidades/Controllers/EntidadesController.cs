using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIEntidades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntidadesController : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioAppService;

        public EntidadesController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;  
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
    }
}
