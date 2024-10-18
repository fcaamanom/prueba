namespace ApiExtranjeros.Models.Expediente
{
    public class HistorialExtranjeroRequest
    {
        /// <summary>
        /// Identificador del puesto migratorio.
        /// </summary>
        public int PuestoId { get; set; }

        /// <summary>
        /// Identificador de la calidad asociada.
        /// </summary>
        public int CalidadId { get; set; }

        /// <summary>
        /// Consecutivo de historial de extranjero.
        /// </summary>
        public int ConsecutivoHistorial { get; set; }

        /// <summary>
        /// Estado del historial de extranjero.
        /// </summary>
        public int EstadoHistorial { get; set; }

        /// <summary>
        /// Usuario que realiza la inserción.
        /// </summary>
        public string Usuario { get; set; } = string.Empty; 

        /// <summary>
        /// Fecha de inserción del historial.
        /// </summary>
        public DateTime FechaInsert { get; set; }
    }

}
