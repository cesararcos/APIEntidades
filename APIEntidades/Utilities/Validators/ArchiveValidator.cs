using APIEntidades.Domain.Dto;
using FluentValidation;

namespace APIEntidades.Utilities.Validators
{
    public class ArchiveValidator : AbstractValidator<int>
    {
        public ArchiveValidator()
        {
            RuleFor(top => top)
                .GreaterThan(0).WithMessage("Debe ser mayor de 0.");
        }
    }
}
