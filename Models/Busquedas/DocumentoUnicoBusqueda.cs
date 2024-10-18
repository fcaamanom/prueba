namespace ApiExtranjeros.Models.Busquedas
{
    public class DocumentoUnicoBusquedaRequest
    {
        public string NumeroDocUnico { get; set; } // Requerido
        public string Funcionario { get; set; }    // Requerido
    }

    public class DocumentoUnicoBusquedaResponse
    {
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string CondicionMigratoria { get; set; }
        public string CondicionLaboral { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaRenovacion { get; set; }
    }

}
