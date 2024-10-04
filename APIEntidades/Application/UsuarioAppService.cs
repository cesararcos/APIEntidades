using APIEntidades.Application.Contracts;
using APIEntidades.Domain.Dto;
using APIEntidades.Utilities.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace APIEntidades.Application
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IValidator<UsuarioDto> _usuarioValidator;
        public UsuarioAppService(IValidator<UsuarioDto> usuarioValidator)
        {
            _usuarioValidator = usuarioValidator;
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
