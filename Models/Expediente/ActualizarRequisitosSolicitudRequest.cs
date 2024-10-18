namespace ApiExtranjeros.Models.Expediente
{
    public class ActualizarRequisitosSolicitudRequest
    {
        /// <summary>
        /// Identificador de la solicitud.
        /// </summary>
        public int SolicitudId { get; set; }

        /// <summary>
        /// Lista de requisitos asociados a la solicitud.
        /// </summary>
        public List<RequisitoSolicitud> Requisitos { get; set; }

        /// <summary>
        /// Usuario que realiza la actualización.
        /// </summary>
        public string Usuario { get; set; }
    }

    public class RequisitoSolicitud
    {
        /// <summary>
        /// Identificador del requisito.
        /// </summary>
        public int RequisitoId { get; set; }

        /// <summary>
        /// Indica si el requisito fue presentado (true) o no (false).
        /// </summary>
        public bool Presento { get; set; }
    }
}
