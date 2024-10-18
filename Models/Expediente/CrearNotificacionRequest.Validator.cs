using FluentValidation;

namespace ApiExtranjeros.Models.Expediente
{
    public class CrearNotificacionRequestValidator : AbstractValidator<CrearNotificacionRequest>
    {
        public CrearNotificacionRequestValidator()
        {
            // Parámetros requeridos
            RuleFor(x => x.IdPuestoNotificacion)
                .GreaterThan(0).WithMessage("El campo 'idPuestoNotificacion' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.TipoNotificacion)
                .NotEmpty().WithMessage("El campo 'tipoNotificacion' es requerido.")
                .Must(t => t == "C" || t == "V")
                .WithMessage("El campo 'tipoNotificacion' debe ser 'C' (Correo electrónico) o 'V' (Ventanilla de servicios).");

            RuleFor(x => x.IdPuestoResolucion)
                .GreaterThan(0).WithMessage("El campo 'idPuestoResolucion' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.NumeroResolucion)
                .GreaterThan(0).WithMessage("El campo 'numeroResolucion' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.TipoResolucion)
                .GreaterThan(0).WithMessage("El campo 'tipoResolucion' es requerido y debe ser mayor a 0.");

            RuleFor(x => x.IdNotificado)
                .NotEmpty().WithMessage("El campo 'idNotificado' es requerido.")
                .MaximumLength(18).WithMessage("El campo 'idNotificado' no debe exceder 18 caracteres.");

            RuleFor(x => x.NombreNotificado)
                .NotEmpty().WithMessage("El campo 'nombreNotificado' es requerido.")
                .MaximumLength(100).WithMessage("El campo 'nombreNotificado' no debe exceder 100 caracteres.")
                .Matches(@"^[A-ZÑ]+( [A-ZÑ]+)*$").WithMessage("El campo 'nombreNotificado' solo debe contener letras mayúsculas y espacios, y no puede iniciar con espacio.");

            RuleFor(x => x.FechaNotificacion)
                .NotEmpty().WithMessage("El campo 'fechaNotificacion' es requerido.");

            RuleFor(x => x.Despacho)
                .NotEmpty().WithMessage("El campo 'despacho' es requerido.")
                .Must(d => d == "Plataforma VUI" || d == "SITLAM" || d == "TRAMITE_YA")
                .WithMessage("El campo 'despacho' debe ser 'Plataforma VUI', 'SITLAM' o 'TRAMITE_YA'.");

            RuleFor(x => x.Acta)
                .NotNull().WithMessage("El campo 'Acta' es requerido.")
                .Must(file => file.Length > 0).WithMessage("El archivo 'Acta' no puede estar vacío.");

            RuleFor(x => x.Funcionario)
                .NotEmpty().WithMessage("El campo 'funcionario' es requerido.")
                .MaximumLength(30).WithMessage("El campo 'funcionario' no debe exceder 30 caracteres.");

            // Parámetros opcionales
            RuleFor(x => x.Observacion)
                .MaximumLength(250).WithMessage("El campo 'observacion' no debe exceder 250 caracteres.")
                .Matches(@"^[A-ZÑ0-9 .,;]*$").WithMessage("El campo 'observacion' contiene caracteres no permitidos.")
                .When(x => !string.IsNullOrEmpty(x.Observacion));

            RuleFor(x => x.Folios)
                .GreaterThanOrEqualTo(0).When(x => x.Folios.HasValue)
                .WithMessage("El campo 'folios' debe ser mayor o igual a 0.");
        }
    }
}
