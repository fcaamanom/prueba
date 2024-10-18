using FluentValidation;

namespace ApiExtranjeros.Models.Expediente
{
    public class CrearApoderadoRequestValidator : AbstractValidator<CrearApoderadoRequest>
    {
        public CrearApoderadoRequestValidator()
        {
            RuleFor(x => x.NombreApoderado)
                .NotEmpty().WithMessage("El campo 'nombreApoderado' es requerido.")
                .MaximumLength(50).WithMessage("El campo 'nombreApoderado' no debe exceder 50 caracteres.");

            RuleFor(x => x.Identificacion)
                .NotEmpty().WithMessage("El campo 'identificacion' es requerido.")
                .MaximumLength(30).WithMessage("El campo 'identificacion' no debe exceder 30 caracteres.");

            RuleFor(x => x.TipoApoderado)
                .NotEmpty().WithMessage("El campo 'tipoApoderado' es requerido.")
                .Must(value => value == "R" || value == "A")
                .WithMessage("El campo 'tipoApoderado' debe ser 'R' (particular) o 'A' (abogado).");

            RuleFor(x => x.Funcionario)
                .NotEmpty().WithMessage("El campo 'funcionario' es requerido.")
                .MaximumLength(30).WithMessage("El campo 'funcionario' no debe exceder 30 caracteres.");
        }
    }
}
