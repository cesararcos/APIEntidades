using APIEntidades.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace APIEntidades.Application.Contracts
{
    public interface IUsuarioAppService
    {
        ResponseDto<bool> Register(UsuarioDto usuario);
        ResponseDto<string> Login(IngresoDto ingreso);
    }
}
