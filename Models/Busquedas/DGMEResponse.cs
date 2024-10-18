namespace ApiExtranjeros.Models.Busquedas
{
    public class DGMEResponse
    {
        public int NumeroExpediente { get; set; }
        public int PuestoMigratorio { get; set; }
        public DateTime FechaExpediente { get; set; }
        public string EstadoExpediente { get; set; }
        public int PuestoCalidad { get; set; }
        public int Calidad { get; set; }
        public string DocumentoUnico { get; set; }
        public int CondicionMigratoria { get; set; }//
        public int CondicionLaboral { get; set; }//
        public DateTime FechaRenovacion { get; set; }//
        public int TipoIdentificacion { get; set; } //Doc_ID puede ser
        public string Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }// Esta puesto con NacionalidadID
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
    }
}
