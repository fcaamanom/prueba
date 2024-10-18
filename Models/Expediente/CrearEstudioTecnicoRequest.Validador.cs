using ApiExtranjeros.Common;
using FluentValidation;

namespace ApiExtranjeros.Models.Expediente
{
    public class CrearEstudioTecnicoRequestValidator : AbstractValidator<CrearEstudioTecnicoRequest>
    {
        private readonly CatalogoValidations _catalogoValidations;
        public CrearEstudioTecnicoRequestValidator(CatalogoValidations catalogoValidations)
        {
            _catalogoValidations = catalogoValidations;
            // Parámetros requeridos
            RuleFor(x => x.IdPuestoEstudio)
                .GreaterThan(0).WithMessage("El campo 'idPuestoEstudio' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.IdPuestoSolicitud)
                .GreaterThan(0).WithMessage("El campo 'idPuestoSolicitud' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.NumeroSolicitud)
                .GreaterThan(0).WithMessage("El campo 'numeroSolicitud' es requerido y debe ser mayor a 0.");
            //Incluido;Franklin
            RuleFor(x => x.EstadoEstudio)
                .Must(_catalogoValidations.BeAValidEstadoEstudio)
                .WithMessage($"El campo 'EstadoEstudio' debe ser un es estados {_catalogoValidations.GetEstadoEstudio()}")
                .MaximumLength(2).WithMessage("El campo 'EstadoEstudio' no debe exceder 2 caracteres.")
                .NotEmpty().WithMessage("El campo 'EstadoEstudio' es requerido.");

            RuleFor(x => x.Pronunciamiento)
                .NotEmpty().WithMessage("El campo 'pronunciamiento' es requerido.");

            RuleFor(x => x.Comentarios)
                .NotEmpty().WithMessage("El campo 'comentarios' es requerido.");

            RuleFor(x => x.Funcionario)
                .NotEmpty().WithMessage("El campo 'funcionario' es requerido.")
                .MaximumLength(30).WithMessage("El campo 'funcionario' no debe exceder 30 caracteres.");

            // Validaciones para campos opcionales
            RuleFor(x => x.Moneda)
                .Must(m => string.IsNullOrEmpty(m) || _catalogoValidations.BeAValidTipoMonedas(m))
                .WithMessage($"El campo 'moneda' debe ser {_catalogoValidations.GetTipoMonedas()} o vacío.");

            RuleFor(x => x.IdApoderado)
                .MaximumLength(20).WithMessage("El campo 'idApoderado' no debe exceder 20 caracteres.");

            RuleFor(x => x.Garante)
                .MaximumLength(80).WithMessage("El campo 'garante' no debe exceder 80 caracteres.");

            RuleFor(x => x.DepositoGarantia)
                .GreaterThanOrEqualTo(0).When(x => x.DepositoGarantia.HasValue)
                .WithMessage("El campo 'depositoGarantia' debe ser un número positivo.");
        }
    }
}
