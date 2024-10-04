using APIEntidades.Domain.Dto;
using FluentValidation;

namespace APIEntidades.Utilities.Validators
{
    public class UsuarioValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioValidator()
        {
            RuleFor(usuario => usuario.Correo)
                .NotEmpty().WithMessage("El correo electrónico es requerido.")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");

            RuleFor(usuario => usuario.Clave)
                .NotEmpty().WithMessage("El nombre es requerido.");
                

            //RuleFor(usuario => usuario.Edad)
            //    .GreaterThan(18).WithMessage("Debe ser mayor de 18 años.")
            //    .LessThan(100).WithMessage("Debe tener menos de 100 años.")
            //    .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres.");
        }
    }
}
