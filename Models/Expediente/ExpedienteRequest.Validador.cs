using ApiExtranjeros.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace ApiExtranjeros.Models.Expediente
{
    public class ExpedienteRequestValidator : AbstractValidator<ExpedienteRequest>
    {
        private readonly CatalogoValidations _catalogoValidations;

        public ExpedienteRequestValidator(CatalogoValidations catalogoValidations, IOptions<Configuration> configuration)
        {
            _catalogoValidations = catalogoValidations;

            RuleFor(x => x.IdPuesto)
            .GreaterThanOrEqualTo(1).WithMessage("El campo IdPuesto debe ser uno válido.")
            .Must(_catalogoValidations.BeAValidPuestoMigratorio).WithMessage("El IdPuesto no existe. ");


            RuleFor(x => x.CondicionMigratoria)
            .GreaterThanOrEqualTo(0).WithMessage("El campo CondicionMigratoria no es válido.")
            .Must(_catalogoValidations.BeAValidCondicionMigratoria).WithMessage("La condición migratoria no es válida.");


            RuleFor(x => x.CondicionLaboral)
            .GreaterThanOrEqualTo(0).WithMessage("El campo CondicionLaboral no es válido.")
            .Must(_catalogoValidations.BeAValidCondicionLaboral).WithMessage("La condición laboral no es válida.");


            RuleFor(x => x.TipoSolicitud)
            .GreaterThanOrEqualTo(0).WithMessage("El campo TipoSolicitud no es válido.")
            .Must(_catalogoValidations.BeAValidTipoSolicitud).WithMessage("El tipo de solicitud no es válida.");


            RuleFor(x => x.FechaNacimiento)
                .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("La fecha de nacimiento no puede ser inferior a 01/01/1900.")
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento no puede ser en el futuro.");

            RuleFor(x => x.Nacionalidad)
                .NotEmpty().WithMessage("El campo Nacionalidad es obligatorio.")
                .Length(3).WithMessage("El campo Nacionalidad debe tener exactamente 3 caracteres.")
                .Must(_catalogoValidations.BeAValidNacionalidad).WithMessage("La nacionalidad ingresada no existe");

            RuleFor(x => x.TipoDocumento)
                .GreaterThan(0).WithMessage("El campo Tipo de Documento es obligatorio.")
                 .Must(_catalogoValidations.BeAValidTipoDocumento).WithMessage("El tipo de documento no es válido.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El campo Nombre es obligatorio.")
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo Nombre solo permite letras, espacios, puntos y la letra 'ñ'.")
                .MinimumLength(2).WithMessage("El campo Nombre no puede tener menos de 2 caracteres.")
                .MaximumLength(30).WithMessage("El campo Nombre no puede exceder los 30 caracteres.");


            RuleFor(x => x.PrimerApellido)
                .NotEmpty().WithMessage("El campo Nombre es obligatorio.")
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo PrimerApellido solo permite letras y espacios.")
                .MinimumLength(2).WithMessage("El campo PrimerApellido debe tener al menos 2 caracteres.")
                .MaximumLength(30).WithMessage("El campo PrimerApellido no puede exceder los 30 caracteres.")
                .When(x => x.Nacionalidad != "IND");

            RuleFor(x => x.SegundoApellido)
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo Segundo Apellido solo permite letras y espacios.")
                .MinimumLength(3).WithMessage("El campo Segundo Apellido debe tener al menos 3 caracteres.")
                .MaximumLength(30).WithMessage("El campo Segundo Apellido no puede exceder los 30 caracteres.")
                .When(x => string.IsNullOrEmpty(x.SegundoApellido) == false || (x.Nacionalidad == "IND" && string.IsNullOrEmpty(x.PrimerApellido)));

            RuleFor(x => x.ConocidoComo)
                .Matches("^[a-zA-Z ]*$").WithMessage("El campo Conocido Como solo permite letras mayúsculas y espacios.")
                .MaximumLength(50).WithMessage("El campo Conocido Como no puede exceder los 50 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.ConocidoComo));

            RuleFor(x => x.Profesion)
                .GreaterThan(1).WithMessage("El campo Profesión es obligatorio.")
                .Must(_catalogoValidations.BeAValidOcupacion).WithMessage("La profesión ingresada no existe.");

            RuleFor(x => x.Sexo)
                .InclusiveBetween(0, 2).WithMessage("El campo Sexo debe ser 0 (Masculino), 1 (Femenino) o 2 (Otro).");

            RuleFor(x => x.CambioGenero)
                .Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        // Asigna el valor 'N' si el campo es nulo o vacío
                        context.InstanceToValidate.CambioGenero = "N";
                    }
                    else if (value.ToUpper() != "S" && value.ToUpper() != "N")
                    {
                        // Si tiene algún otro valor distinto de 'S' o 'N', agrega el mensaje de error
                        context.AddFailure("El campo Cambio de Género debe ser 'S' (Sí) o 'N' (No), sin importar mayúsculas o minúsculas.");
                    }
                });


            RuleFor(x => x.EstadoCivil)
                .NotEmpty().WithMessage("El campo Estado Civil es obligatorio.")
                .Must(_catalogoValidations.BeAValidEstadoCivil).WithMessage("El campo Estado Civil debe ser 'S' (Soltero), 'C' (Casado), 'D' (Divorciado), 'U' (Unión Libre) o 'V' (Viudo).");

            RuleFor(x => x.NivelAcademico)
                .NotEmpty().WithMessage("El campo Nivel Académico es obligatorio.")
                .Must(_catalogoValidations.BeAValidNivelAcademico).WithMessage("El campo Nivel Académico debe ser 'P', 'S', 'T', 'U' o 'N'.");


            RuleFor(x => x.LugarNacimiento)
                .Matches("^[a-zA-Z0-9 .,;]+$").WithMessage("El campo LugarNacimiento solo permite letras, números, espacios, y los caracteres .,;")
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.LugarNacimiento)).WithMessage("El campo LugarNacimiento debe tener al menos 3 caracteres.")
                .MaximumLength(50).WithMessage("El campo LugarNacimiento no puede exceder los 50 caracteres.");


            RuleFor(x => x.NumeroDocumento)
                .Matches("^[a-zA-Z0-9]+$").WithMessage("El campo Número de Documento no es válido de ser ingresar solo números y letras.")
                 .MaximumLength(30).WithMessage("El Número de Documento no puede exceder los 30 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.NumeroDocumento));

            RuleFor(x => x.DocumentoUnico)
                 .MaximumLength(50).WithMessage("El Número de Documento único no puede exceder los 50 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.DocumentoUnico));

            RuleFor(x => x.Consentimiento)
            .NotEmpty().WithMessage("El campo Consentimiento es obligatorio.")
            .Matches("^[sSnN]$").WithMessage("El campo Consentimiento debe ser 'S' o 'N'.");


            RuleFor(x => x.NombrePadre)
                .NotEmpty().WithMessage("El campo NombrePadre es obligatorio.")
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo NombrePadre solo permite letras y espacios.")
                .MinimumLength (2).WithMessage("El campo NombrePadre debe contener al menos 2 caracteres.")
                .MaximumLength(30).WithMessage("El campo NombrePadre no puede exceder los 30 caracteres.");

            RuleFor(x => x.PrimerApellidoPadre)
                .NotEmpty().WithMessage("El campo PrimerApellidoPadre es obligatorio.")
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo PrimerApellidoPadre solo permite letras y espacios.")
                .MinimumLength(2).WithMessage("El campo PrimerApellidoPadre debe tener al menos 2 caracteres.")
                .MaximumLength(100).WithMessage("El campo PrimerApellidoPadre no puede exceder los 100 caracteres.")
                .When(x => x.Nacionalidad != "IND");

            RuleFor(x => x.SegundoApellidoPadre)
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo SegundoApellidoPadre solo permite letras y espacios.")
                .MinimumLength(2).WithMessage("El campo SegundoApellidoPadre debe tener al menos 2 caracteres.")
                .MaximumLength(10).WithMessage("El campo SegundoApellidoPadre no puede exceder los 100 caracteres.")
                .When(x => string.IsNullOrEmpty(x.PrimerApellidoPadre) == false || (x.Nacionalidad == "IND" && string.IsNullOrEmpty(x.PrimerApellidoPadre)));

            RuleFor(x => x.NombreMadre)
             .NotEmpty().WithMessage("El campo NombreMadre es obligatorio.")
             .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo NombreMadre solo permite letras y espacios.")
             .MinimumLength(2).WithMessage("El campo NombreMadre debe tener al menos 2 caracteres.")
             .MaximumLength(30).WithMessage("El campo NombreMadre no puede exceder los 30 caracteres.");

            RuleFor(x => x.PrimerApellidoMadre)
                .NotEmpty().WithMessage("El campo PrimerApellidoMadre es obligatorio.")
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo PrimerApellidoMadre solo permite letras y espacios.")
                .MinimumLength(3).WithMessage("El campo PrimerApellidoMadre debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El campo PrimerApellidoMadre no puede exceder los 100 caracteres.")
                .When(x => x.Nacionalidad != "IND");

            RuleFor(x => x.SegundoApellidoMadre)
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo SegundoApellidoMadre solo permite letras y espacios.")
                .MinimumLength(3).WithMessage("El campo SegundoApellidoMadre debe tener al menos 3 caracteres.")
                .MaximumLength(10).WithMessage("El campo SegundoApellidoMadre no puede exceder los 100 caracteres.")
                .When(x => string.IsNullOrEmpty(x.SegundoApellidoMadre) == false || (x.Nacionalidad == "IND" && string.IsNullOrEmpty(x.PrimerApellidoMadre)));

            RuleFor(x => x.DonaOrganos)
            .InclusiveBetween(0, 3).WithMessage("El campo DonaOrganos debe ser 0 (No quiere donar órganos ni tejido), 1 (Quiere donar órganos y tejido), 2 (Quiere donar órganos, pero no tejido), o 3 (No quiere donar órganos, pero sí tejido).\n Nota: El dato predeterminado sería: “3” Persona no quiere donar órganos, pero sí tejido, según normativa del ministerio de salud vigente.");

            RuleFor(x => x.FechaDonacion)
             .GreaterThanOrEqualTo(new DateTime(1948, 1, 1)).WithMessage("La fecha de ingreso no puede ser inferior al 01/01/1948.")
             .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de donación no puede ser mayor a la fecha de hoy.")
             .When(x => x.DonaOrganos != 0);

            RuleFor(x => x.TieneIncapacidad)
                .InclusiveBetween(0, 1).WithMessage("El campo Tiene Incapacidad debe ser 0 (No) o 1 (Sí).");

            RuleFor(x => x.MenorEdad)
                .NotNull().WithMessage("El campo Menor Edad es obligatorio.");

            RuleFor(x => x.PatriaPotestad)
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo Patria Potestad solo permite letras  y espacios.")
                 .MinimumLength(2).WithMessage("El campo PatriaPotestad no puede contener menos de 2 caracteres.")
                 .MaximumLength(50).WithMessage("El campo PatriaPotestad no puede exceder los 50 caracteres.")
                .When(x => x.MenorEdad);

            RuleFor(x => x.RepresentanteLegal)
                .Matches("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ. ]+$").WithMessage("El campo Representante Legal solo permite letras  y espacios.")
                .MinimumLength(2).WithMessage("El campo RepresentanteLegal no puede contener menos de 2 caracteres.")
                .MaximumLength(50).WithMessage("El campo RepresentanteLegal no puede exceder los 50 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.RepresentanteLegal));

            RuleFor(x => x.FechaSalidaPaisOrigen)
                .GreaterThanOrEqualTo(x => x.FechaNacimiento).WithMessage("La fecha de salida del país no puede ser anterior a la fecha de nacimiento.")
                .LessThanOrEqualTo(x => x.FechaIngresoCR).WithMessage("La fecha de salida del país no puede ser mayor a la fecha de ingreso a Costa Rica.")
                .When(x => x.FechaSalidaPaisOrigen > DateTime.MinValue);

            RuleFor(x => x.FechaIngresoCR)
                .GreaterThanOrEqualTo(new DateTime(1948, 1, 1)).WithMessage("La fecha de ingreso no puede ser inferior al 01/01/1948.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser mayor a la fecha actual.");

            RuleFor(x => x.PuntoIngresoCR)
              .GreaterThanOrEqualTo(1).WithMessage("El campo PuntoIngresoCR debe ser valor un válido.")
              .Must(_catalogoValidations.BeAValidPuestoMigratorio).WithMessage("El PuntoIngresoCR no existe. ");


            RuleFor(x => x.TelefonoHabitacion)
                .Matches("^[0-9]+$").WithMessage("El campo Teléfono de Habitación solo permite números.")
                .MaximumLength(15).WithMessage("El campo TelefonoHabitacion no puede exceder los 15 números.")
                .When(x => !string.IsNullOrEmpty(x.TelefonoHabitacion));


            RuleFor(x => x.TelefonoCelular)
             .Matches("^[0-9]+$").WithMessage("El campo Teléfono celular solo permite números.")
             .MaximumLength(15).WithMessage("El campo TelefonoCelular no puede exceder los 15 números.")
             .When(x => !string.IsNullOrEmpty(x.TelefonoCelular));


            RuleFor(x => x.TelefonoTrabajo)
                .Matches("^[0-9]+$").WithMessage("El campo Teléfono de Trabajo solo permite números.")
                .MaximumLength(15).WithMessage("El campo TelefonoTrabajo no puede exceder los 15 números.")
                .When(x => !string.IsNullOrEmpty(x.TelefonoTrabajo));

            RuleFor(x => x.Fax)
                .Matches("^[0-9]+$").WithMessage("El campo Fax solo permite números.")
                .MaximumLength(15).WithMessage("El campo Fax no puede exceder los 15 números.")
                .When(x => !string.IsNullOrEmpty(x.Fax));

            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El campo Correo es obligatorio.")
                .EmailAddress().WithMessage("El campo Correo debe tener un formato válido.");


            RuleFor(x => x.Provincia)
                .GreaterThanOrEqualTo(1).WithMessage("El Id de la provincia debe ser válido.")
                .Must(_catalogoValidations.BeAValidProvincia).WithMessage("La provincia no es válida.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Canton)
                        .GreaterThanOrEqualTo(1).WithMessage("El Id del cantón debe ser válido.")
                        .Must((request, cantonId) => _catalogoValidations.BeAValidCanton(cantonId, request.Provincia))
                        .WithMessage("El cantón no es válido para la provincia especificada.")
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Distrito)
                                .GreaterThanOrEqualTo(1).WithMessage("El Id del distrito debe ser válido.")
                                .Must((request, distritoId) => _catalogoValidations.BeAValidDistrito(distritoId, request.Canton))
                                .WithMessage("El distrito no es válido para el cantón especificado.");
                        });
                });


            RuleFor(x => x.TipoDireccion)
                .NotEmpty().WithMessage("El campo Tipo Dirección es obligatorio.")
                .Matches("[CTO]").WithMessage("El campo Tipo Dirección debe ser 'C' (Casa), 'T' (Trabajo) o 'O' (Otro).");

            RuleFor(x => x.Direccion)
                .Matches("^[a-zA-Z0-9 .,;]*$").WithMessage("El campo Dirección solo permite letras mayúsculas, números, espacios y algunos caracteres especiales.")
                .MaximumLength(200).WithMessage("El campo Dirección no puede exceder los 200 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Direccion));


            RuleFor(x => x.PlanReqNaturaleza)
                .NotEmpty().WithMessage("El campo Naturaleza es requerido.")
                .Must(value => new[] { "N", "R", "D" }.Contains(value.ToUpper()))
                .WithMessage("El campo Naturaleza solo acepta los valores: 'N' para Nuevo, 'R' para Renovación, o 'D' para Duplicado.");


            When(x => x.TipoSolicitud != 0 && x.CondicionMigratoria != 0 && x.CondicionLaboral != 0 && !string.IsNullOrEmpty(x.PlanReqNaturaleza), () =>
            {
                RuleFor(x => x.plantillaRequisitos)
                    .Must((request, plantillaRequisitos) =>
                    {
                        var requisitosValidos = _catalogoValidations.GetPlantillasTodas(request.TipoSolicitud, request.CondicionMigratoria, request.CondicionLaboral, request.PlanReqNaturaleza);

                        // Verificar si existen requisitos válidos y que todos los requisitos sean válidos en la lista
                        if (requisitosValidos.Count == 0)
                            return false;

                        // Crear un conjunto de IDs de los requisitos válidos
                        var idsRequisitosValidos = requisitosValidos.Select(m => m.IdDocumento).ToHashSet();

                        // Verificar que todos los requisitos en plantillaRequisitos sean válidos y que el campo 'Registrado' tenga un valor permitido
                        if (!plantillaRequisitos.All(pr =>
                            idsRequisitosValidos.Contains(pr.DocId) &&
                            new[] { "S", "O", "N" }.Contains(pr.Registrado.ToUpper())
                        ))
                        {
                            return false;
                        }

                        // Verificar que no haya duplicados en plantillaRequisitos
                        if (plantillaRequisitos.Select(pr => pr.DocId).Distinct().Count() != plantillaRequisitos.Count)
                        {
                            return false;
                        }

                        return true;
                    })
                    .WithMessage("Uno o más IDs de requisitos no existen en la lista de requisitos permitidos, el campo 'Registrado' tiene un valor no permitido, o hay requisitos duplicados o faltantes.");
            });


        }
    }
}
