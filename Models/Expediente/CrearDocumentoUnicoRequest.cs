namespace ApiExtranjeros.Models.Expediente
{
    public class CrearDocumentoUnicoRequest
    {
        // Parámetros requeridos
        public int IdPuestoCalidad { get; set; } // Numeric(4,0)
        public long NumeroCalidad { get; set; } // Numeric(20,0)
        public int IdPuestoSolicitud { get; set; } // Numeric(4,0)
        public long NumeroSolicitud { get; set; } // Numeric(18,0)
        public DateTime FechaRenovacion { get; set; } // DateTime
        public int IdCondicionMigratoria { get; set; } // Integer
        public int IdCondicionLaboral { get; set; } // Integer
        public string Nacionalidad { get; set; } // Char(4)
        public string Funcionario { get; set; } // Varchar(30)
    }

}
