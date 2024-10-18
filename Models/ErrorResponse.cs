namespace ApiExtranjeros.Models
{
    public class ErrorResponse
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public object Detalles { get; set; }
    }
}

