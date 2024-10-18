namespace ApiExtranjeros.Models.Expediente
{
    public class CrearEstudioTecnicoRequest
    {
        // Parámetros requeridos
        public int IdPuestoEstudio { get; set; }
        public int IdPuestoSolicitud { get; set; }
        public long NumeroSolicitud { get; set; }
        public string EstadoEstudio { get; set; }
        public short Pronunciamiento { get; set; }
        public string Comentarios { get; set; }
        public string Funcionario { get; set; }

        // Parámetros opcionales
        public string IdApoderado { get; set; } // Char(20)
        public string Garante { get; set; } // Varchar(80)
        public decimal? DepositoGarantia { get; set; } // Numeric(10,0)
        public string Moneda { get; set; } // Char(3), "COL" o "DOL"
    }
}
