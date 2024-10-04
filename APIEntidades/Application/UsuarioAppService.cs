using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using APIEntidades.Domain.Entities;
using APIEntidades.Infrastructure.DataAccess;
using APIEntidades.Infrastructure.Helpers;
using APIEntidades.Utilities;
using APIEntidades.Utilities.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace APIEntidades.Application
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly EntidadesDbContext _context;
        private readonly IValidator<UsuarioDto> _usuarioValidator;
        private readonly IValidator<IngresoDto> _ingresoValidator;
        private readonly IUtilities _utilities;
        public UsuarioAppService(EntidadesDbContext context, IValidator<UsuarioDto> usuarioValidator, IValidator<IngresoDto> ingresoValidator, IUtilities utilities)
        {
            _context = context;
            _usuarioValidator = usuarioValidator;
            _ingresoValidator = ingresoValidator;
            _utilities = utilities;
        }

        public ResponseDto<bool> Register(UsuarioDto usuario)
        {
            try
            {
                var result = _usuarioValidator.Validate(usuario);

                if (!result.IsValid)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = result.Errors.First().ErrorMessage
                    };
                }

                var findUser = _context?.Usuarios?.FirstOrDefault(x => x.Correo == usuario.Correo) ?? null;

                if (findUser != null)
                {
                    return new ResponseDto<bool>
                    {
                        Success = false,
                        ErrorMessage = Constants.USER_EXIST
                    };
                }

                Usuarios user = new()
                {
                    Id = Guid.NewGuid(),
                    Correo = usuario.Correo,
                    Clave = usuario.Clave,
                    Usuario = usuario.Usuario
                };

                _context!.Usuarios.Add(user);
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

        public ResponseDto<string> Login(IngresoDto ingreso)
        {
            try
            {
                var result = _ingresoValidator.Validate(ingreso);

                if (!result.IsValid)
                {
                    return new ResponseDto<string>
                    {
                        Success = false,
                        ErrorMessage = result.Errors.First().ErrorMessage
                    };
                }

                var findUser = _context?.Usuarios?.FirstOrDefault(x => x.Correo == ingreso.Correo && x.Clave == ingreso.Clave) ?? null;

                if (findUser == null)
                {
                    return new ResponseDto<string>
                    {
                        Success = false,
                        ErrorMessage = Constants.USER_NOT_EXIST
                    };
                }

                var token = _utilities.GenerarToken(ingreso.Correo!);

                return new ResponseDto<string>
                {
                    Success = true,
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    Success = false,
                    ErrorMessage = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
