namespace ApiExtranjeros.Models.Expediente
{
    public class SolicitudRequest
    {
        /// <summary>
        /// Identificador del puesto migratorio.
        /// </summary>
        public int PuestoId { get; set; }

        /// <summary>
        /// Identificador del expediente asociado a la solicitud.
        /// </summary>
        public int ExpedienteId { get; set; }

        /// <summary>
        /// Identificador de la calidad asociada.
        /// </summary>
        public int CalidadId { get; set; }

        /// <summary>
        /// Identificador del tipo de solicitud.
        /// </summary>
        public int TipoSolicitudId { get; set; }

        /// <summary>
        /// Identificador de la condición migratoria.
        /// </summary>
        public int CondicionMigratoriaId { get; set; }

        /// <summary>
        /// Identificador de la condición laboral.
        /// </summary>
        public int CondicionLaboralId { get; set; }

        /// <summary>
        /// Fecha en que se realiza la solicitud.
        /// </summary>
        public DateTime FechaSolicitud { get; set; }

        /// <summary>
        /// Estado de la solicitud (Ejemplo: Activo, Inactivo).
        /// </summary>
        public string EstadoSolicitud { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de trámite (Ejemplo: Nuevo, Renovación).
        /// </summary>
        public string TipoTramite { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de solicitante (Ejemplo: Persona física, Persona jurídica).
        /// </summary>
        public string TipoSolicitante { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que realiza la inserción.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;
    }

}
