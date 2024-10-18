using FluentValidation;

namespace ApiExtranjeros.Models.Expediente
{
    public class ActualizarDocumentoUnicoRequestValidator : AbstractValidator<ActualizarDocumentoUnicoRequest>
    {
        public ActualizarDocumentoUnicoRequestValidator()
        {
            // Parámetros requeridos
            RuleFor(x => x.IdPuestoCalidad)
                .GreaterThan(0).WithMessage("El campo 'idPuestoCalidad' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.NumeroCalidad)
                .GreaterThan(0).WithMessage("El campo 'numeroCalidad' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.IdPuestoSolicitud)
                .GreaterThan(0).WithMessage("El campo 'idPuestoSolicitud' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.NumeroSolicitud)
                .GreaterThan(0).WithMessage("El campo 'numeroSolicitud' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.FechaRenovacion)
                .NotEmpty().WithMessage("El campo 'fechaRenovacion' es requerido.");

            RuleFor(x => x.IdCondicionMigratoria)
                .GreaterThan(0).WithMessage("El campo 'idCondicionMigratoria' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.IdCondicionLaboral)
                .GreaterThan(0).WithMessage("El campo 'IdCondicionLaboral' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.Funcionario)
                .NotEmpty().WithMessage("El campo 'Funcionario' es requerido.")
                .MaximumLength(30).WithMessage("El campo 'Funcionario' no debe exceder 30 caracteres.");
        }
    }
}
