namespace ApiExtranjeros.Models.Expediente
{
    public class CrearNotificacionRequest
    {
        // Parámetros requeridos
        public int IdPuestoNotificacion { get; set; }
        public string TipoNotificacion { get; set; } // 'C' o 'V'
        public int IdPuestoResolucion { get; set; }
        public long NumeroResolucion { get; set; }
        public int TipoResolucion { get; set; }
        public string IdNotificado { get; set; } // Char(18)
        public string NombreNotificado { get; set; } // Char(100)
        public DateTime FechaNotificacion { get; set; }
        public string Despacho { get; set; } // Varchar(100)
        public IFormFile Acta { get; set; }
        public string Funcionario { get; set; }

        // Parámetros opcionales
        public string Observacion { get; set; } // Char(250)
        public int? Folios { get; set; }
    }
}
