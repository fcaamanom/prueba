namespace ApiExtranjeros.Models.Busquedas
{
    public class ConsecutivoRequest
    {
        public int? IdPuestoMigratorio { get; set; }
        public string NombreTabla { get; set; }
        public string TipoResolucion { get; set; }

        public long ConsecutivoNuevo { get; set; }
        public string Nacionalidad { get; set; }
    }
}
