namespace ApiExtranjeros.Models.Expediente
{
    public class CalidadRequest
    {
        /// <summary>
        /// Identificador del puesto migratorio.
        /// </summary>
        public int PuestoId { get; set; }

        /// <summary>
        /// Identificador de la nacionalidad.
        /// </summary>
        public int NacionalidadId { get; set; }

        /// <summary>
        /// Identificador de la ocupación.
        /// </summary>
        public int OcupacionId { get; set; }

        /// <summary>
        /// Identificador del tipo de documento.
        /// </summary>
        public int DocumentoId { get; set; }

        /// <summary>
        /// Primer apellido de la persona.
        /// </summary>
        public string PrimerApellido { get; set; } = string.Empty;

        /// <summary>
        /// Segundo apellido de la persona.
        /// </summary>
        public string SegundoApellido { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento de la persona.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Lugar de nacimiento.
        /// </summary>
        public string LugarNacimiento { get; set; } = string.Empty;

        /// <summary>
        /// Estado civil de la persona.
        /// </summary>
        public string EstadoCivil { get; set; } = string.Empty;

        /// <summary>
        /// Nivel académico de la persona.
        /// </summary>
        public string NivelEducacion { get; set; } = string.Empty;

        /// <summary>
        /// Número de documento de la persona.
        /// </summary>
        public string NumeroDocumento { get; set; } = string.Empty;
        /// <summary>
        /// Teléfono de habitación.
        /// </summary>
        public string TelefonoHabitacion { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono celular.
        /// </summary>
        public string TelefonoCelular { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que realiza la inserción.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;
    }
}
