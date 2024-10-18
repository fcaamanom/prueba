namespace ApiExtranjeros.Models.Busquedas
{
    public class EmpresaBusquedaRequest
    {
        public int TipoBusqueda { get; set; } // 0, 1, 2, o 3
        public string NombreEmpresa { get; set; } // Requerido para tipo 0 y 2
        public string AliasEmpresa { get; set; } // Requerido para tipo 0 y 3
        public string CedulaJuridica { get; set; } // Requerido para tipo 0 y 1
        public string Funcionario { get; set; } // Requerido
    }

    public class EmpresaBusquedaResponse
    {
        public int IdConsecutivoEmpresa { get; set; }
        public string CedulaJuridica { get; set; }
        public string NombreEmpresa { get; set; }
        public string AliasEmpresa { get; set; }
        public string GiroComercial { get; set; }
        public string IdRepresentanteLegal { get; set; }
        public string NombreRepresentante { get; set; }
        public string CategoriaEmpresa { get; set; }
        public DateTime FechaRenovacion { get; set; }
    }
}
