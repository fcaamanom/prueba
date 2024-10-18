namespace ApiExtranjeros.Models
{
    public class RegistroApi
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        public string UsuarioId { get; set; } = string.Empty;

        public string PuntoFinal { get; set; } = string.Empty;

        public string MetodoHttp { get; set; } = string.Empty;

        public string EncabezadosSolicitud { get; set; } = string.Empty;

        public string CuerpoSolicitud { get; set; } = string.Empty;

        public int CodigoEstadoRespuesta { get; set; }

        public string CuerpoRespuesta { get; set; } = string.Empty;

        public string MensajeExcepcion { get; set; } = string.Empty;

        public string DireccionIp { get; set; } = string.Empty;

        public string AgenteUsuario { get; set; } = string.Empty;

        public int TiempoEjecucion { get; set; }

        public string TrazaPila { get; set; } = string.Empty; // Nueva propiedad para el stack trace

    }

}
