using FluentValidation;

namespace ApiExtranjeros.Models.Expediente
{
    public class CrearResolucionRequestValidator : AbstractValidator<CrearResolucionRequest>
    {
        private readonly CatalogoValidations _catalogoValidations;

        public CrearResolucionRequestValidator(CatalogoValidations catalogoValidations)
        {
            _catalogoValidations = catalogoValidations;

            // Diccionario de pronunciamientos válidos y su descripción
            var pronunciamientosValidos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                                                                            {
                                                                                { "CU", "Acumulación" },
                                                                                { "AP", "Aprobada" },
                                                                                { "AM", "Aprobar por TAM" },
                                                                                { "AS", "Archivo solicitud" },
                                                                                { "AL", "Autorizar Lab Sol Refugio" },
                                                                                { "PP", "Autorizar Plazo Previos" },
                                                                                { "CC", "Cambio Calidad" },
                                                                                { "CD", "Cambio de Condición" },
                                                                                { "CS", "Cambio Subcategoría" },
                                                                                { "CA", "Cancelación" },
                                                                                { "CF", "Cancelación en Firme" },
                                                                                { "CE", "CASO ESPECIAL" },
                                                                                { "DN", "Denegada" },
                                                                                { "DF", "Denegatoria Firme" },
                                                                                { "DE", "Desistir Gestión" },
                                                                                { "DR", "Desistir Revocatoria" },
                                                                                { "ET", "Emplazar al TAM" },
                                                                                { "EP", "Expulsión del país" },
                                                                                { "IP", "Inadmisibilidad" },
                                                                                { "IF", "Informe" },
                                                                                { "IC", "Inicio Procedimiento" },
                                                                                { "LC", "Liberación de Condición" },
                                                                                { "MA", "Mantener" },
                                                                                { "NA", "Nueva Gestión/Aprobado" },
                                                                                { "ND", "Nueva Gestión/Denegado" },
                                                                                { "PR", "Previo" },
                                                                                { "PC", "Previo CCSS" },
                                                                                { "AR", "Previo CCSS Refugiado" },
                                                                                { "PA", "Previo Prórroga Plazo" },
                                                                                { "AC", "Reactivar" },
                                                                                { "RG", "Rebajo Garantía" },
                                                                                { "RE", "Rechazo" },
                                                                                { "RF", "Rechazo Firme" },
                                                                                { "RU", "Rechazo Nulidad" },
                                                                                { "RN", "Renovación" },
                                                                                { "UN", "Renuncia" },
                                                                                { "RP", "Reposición" },
                                                                                { "VO", "Revocar" },
                                                                                { "MI", "Revocar Ministro" },
                                                                                { "SU", "Suspensión" }
                                                                            };




            // Parámetros requeridos
            RuleFor(x => x.IdPuestoCalidad)
                .GreaterThan(0).WithMessage("El campo 'idPuestoCalidad' es obligatorio y debe ser mayor a 0.")
                .Must(_catalogoValidations.BeAValidPuestoMigratorio).WithMessage("El IdPuestoCalidad no existe.");

            RuleFor(x => x.NumeroCalidad)
                .GreaterThan(0).WithMessage("El campo 'numeroCalidad' es obligatorio y debe ser mayor a 0.").
                Must(_catalogoValidations.BeAValidCalidad).WithMessage("El número de calidad ingresado no existe.");

            RuleFor(x => x.IdPuestoSolicitud)
                .GreaterThan(0).WithMessage("El campo 'idPuestoSolicitud' es obligatorio y debe ser mayor a 0.")
                .Must(_catalogoValidations.BeAValidPuestoMigratorio).WithMessage("El IdPuestoSolicitud no existe.");

            RuleFor(x => x.NumeroSolicitud)
                .GreaterThan(0).WithMessage("El campo 'numeroSolicitud' es obligatorio y debe ser mayor a 0."); // validar con la tabla de solicitudes
                .Must(_catalogoValidations.BeAValidSolicitud).WithMessage("El número de solicitud ingresado no existe.");

            RuleFor(x => x.Identificador) 
                .Equal(2).WithMessage("El campo 'identificador' es obligatorio");// Tipo de resolución Catálogo
                .Must(_catalogoValidations.BeAValidResolucion).WithMessage("El indentificador de la resolución no es valor válido.");

            RuleFor(x => x.Pronunciamiento)
            .NotNull().WithMessage("El campo 'pronunciamiento' es obligatorio.")
            .Must(value => pronunciamientosValidos.ContainsKey(value))
            .WithMessage($"El pronunciamiento no es válido. Debe ser uno de los siguientes: {string.Join(", ", pronunciamientosValidos.Select(p => $"{p.Key} ({p.Value})"))}.");

            RuleFor(x => x.IdFirma)
                .GreaterThan(0).WithMessage("El campo 'idFirma' es obligatorio y debe ser mayor a 0.");

            RuleFor(x => x.MayoriaEdad)
                .InclusiveBetween(0, 1).WithMessage("El campo 'mayoriaEdad' es obligatorio y debe ser 0 o 1.");

            /*RuleFor(x => x.Funcionario)
                .NotEmpty().WithMessage("El campo 'Funcionario' es obligatorio.")
                .MaximumLength(30).WithMessage("El campo 'Funcionario' no debe exceder 30 caracteres.");*/ //el funcionario se setea en el request

            // Validación del archivo PDF
            RuleFor(x => x.ResolucionPdf)
                .Must(file => file == null || file.Length > 0).WithMessage("El archivo 'resolucionPdf' debe ser un PDF válido.")
                .Must(file => file == null || file.Length <= 2 * 1024 * 1024) // Máximo 2 MB
                .WithMessage("El archivo 'resolucionPdf' no puede superar los 2 MB.");

            // Validaciones para campos opcionales (ignorando mayúsculas y minúsculas)
            RuleFor(x => x.Vinculo)
                .Must(v => string.IsNullOrEmpty(v) || new[] { "HE", "MA", "PA", "HI", "ES" }.Contains(v.ToUpper()))
                .WithMessage("El campo 'vinculo' debe ser uno de los siguientes valores: HE (Hermano), MA (Madre), PA (Padre), HI (Hijo/a), ES (Esposo/a).");

            RuleFor(x => x.Motivo)
                .Must(m => string.IsNullOrEmpty(m) || new[] { "MUE", "NAT", "CCT", "RPC", "RST", "ATP", "OTR" }.Contains(m.ToUpper()))
                .WithMessage("El campo 'motivo' debe ser uno de los siguientes valores: MUE (Muerte), NAT (Naturalización), CCT (Cambio de Categoría), RPC (Repatriación), RST (Reasentamiento), ATP (Antecedentes Penales), OTR (Otros).");

            // Validación de idPuestoEstudio opcional
            RuleFor(x => x.IdPuestoEstudio)
                .GreaterThan(0).When(x => x.IdPuestoEstudio.HasValue)
                .WithMessage("El campo 'idPuestoEstudio' debe ser mayor a 0 cuando esté presente.")
                .Must(_catalogoValidations.BeAValidPuestoMigratorio).When(x => x.IdPuestoEstudio.HasValue)
                .WithMessage("El IdPuestoEstudio no existe.");

            // Validación de idEstudioTecnico opcional
            RuleFor(x => x.IdEstudioTecnico)
                .GreaterThan(0).When(x => x.IdEstudioTecnico.HasValue)
                .WithMessage("El campo 'idEstudioTecnico' debe ser mayor a 0 cuando esté presente.");

        }
    }
}
