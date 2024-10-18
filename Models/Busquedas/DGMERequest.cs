namespace ApiExtranjeros.Models.Busquedas
{
    public class DGMERequest
    {
        public int TipoBusqueda { get; set; }  // 0, 1, 2, 3 o 4
        public string NumeroExpediente { get; set; }
        public int? PuestoMigratorio { get; set; }
        public int? TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
    }
}
