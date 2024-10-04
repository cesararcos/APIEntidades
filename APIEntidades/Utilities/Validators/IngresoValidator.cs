using APIEntidades.Domain.Dto;
using FluentValidation;

namespace APIEntidades.Utilities.Validators
{
    public class IngresoValidator : AbstractValidator<IngresoDto>
    {
        public IngresoValidator()
        {
            RuleFor(usuario => usuario.Correo)
                .NotEmpty().WithMessage("El correo electrónico es requerido.")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");

            RuleFor(usuario => usuario.Clave)
                .NotEmpty().WithMessage("La clave es requerida.");
        }
    }
}
