namespace ApiExtranjeros.Models.Expediente
{
    public class DireccionRequest
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
        /// Identificador de la provincia.
        /// </summary>
        public int ProvinciaId { get; set; }

        /// <summary>
        /// Identificador del cantón.
        /// </summary>
        public int CantonId { get; set; }

        /// <summary>
        /// Identificador del distrito.
        /// </summary>
        public int DistritoId { get; set; }

        /// <summary>
        /// Otras señas adicionales de la dirección.
        /// </summary>
        public string OtrasSenales { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de dirección (Ejemplo: Domicilio, Oficina).
        /// </summary>
        public string TipoDireccion { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que realiza la inserción.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;
    }

}
