namespace ApiExtranjeros.Models.Expediente
{
    public class CrearApoderadoRequest
    {
        // Parámetros requeridos
        public string NombreApoderado { get; set; }
        public string Identificacion { get; set; }
        public string TipoApoderado { get; set; } // 'R' -> particular, 'A' -> abogado
        public string Funcionario { get; set; }
    }
}
