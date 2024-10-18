namespace ApiExtranjeros.Models.Busquedas
{
    public class PlantillaRequisitoResponse
    {
        public int IdTipoSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public int IdCondicionMigratoria { get; set; }
        public string CondicionMigratoria { get; set; }
        public int IdCondicionLaboral { get; set; }
        public string CondicionLaboral { get; set; }
        public int IdDocumento { get; set; }
        public string Documento { get; set; }
        public string NaturalezaTramite { get; set; }
        public string RequisitoRequerido { get; set; }
    }
}