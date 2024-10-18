using System.ComponentModel.DataAnnotations;

namespace ApiExtranjeros.Models.Busquedas
{
    public class DepositoBusquedaRequest
    {

        public string numeroDeposito { get; set; } // Requerido, maximumLength: 200

        public DateTime fechaDeposito { get; set; } // Requerido

        public string Funcionario { get; set; } // Requerido, maximumLength: 30
    }


    public class DepositoBusquedaResponse
    {
        public string tipo { get; set; }
        public string numeroDeposito { get; set; }
        public string cuentaBancaria { get; set; }
        public string identificacion { get; set; }
        public string Nombre { get; set; }
        public string Observaciones { get; set; }
        public decimal monto { get; set; }
        public string tramite { get; set; }
        public DateTime fechadeposito { get; set; }
        public string movutilizado { get; set; }
        public int estado { get; set; }
    }
}
