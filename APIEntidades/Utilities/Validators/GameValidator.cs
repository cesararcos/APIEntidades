using APIEntidades.Domain.Dto;
using FluentValidation;

namespace APIEntidades.Utilities.Validators
{
    public class GameValidator : AbstractValidator<VideoJuegosDto>
    {
        public GameValidator()
        {
            RuleFor(usuario => usuario.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido.");

            RuleFor(usuario => usuario.Compania)
                .NotEmpty().WithMessage("La compania es requerida.");

            RuleFor(usuario => usuario.Ano)
                .NotEmpty().WithMessage("El ano es requerido.");

            RuleFor(usuario => usuario.Precio)
                .NotEmpty().WithMessage("El precio es requerido.");

            RuleFor(usuario => usuario.Puntaje)
                .NotEmpty().WithMessage("El puntaje es requerido.");


            //RuleFor(usuario => usuario.Edad)
            //    .GreaterThan(18).WithMessage("Debe ser mayor de 18 años.")
            //    .LessThan(100).WithMessage("Debe tener menos de 100 años.")
            //    .EmailAddress().WithMessage("El correo electrónico no es válido.")
            //    .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres.");
        }
    }
}
