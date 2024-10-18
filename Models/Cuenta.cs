using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Cuenta
    {

        public char Tipo { get; set; }
        public string Cuenta_Bancaria { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Observacion { get; set; }
        public double Monto { get; set; }
        public bool Estado { get; set; }


        public static Cuenta cargarCuenta(DataRow fila)
        {
            Cuenta nuevo = new Cuenta();
            try
            {
                nuevo.Tipo = Convert.ToChar(fila[0]);
                nuevo.Cuenta_Bancaria = Convert.ToString(fila[1]);
                nuevo.Identificacion = Convert.ToString(fila[2]);
                nuevo.Nombre = Convert.ToString(fila[3]);
                nuevo.Observacion = Convert.ToString(fila[4]);
                nuevo.Monto = Convert.ToDouble(fila[5]);
                nuevo.Estado = Convert.ToBoolean(fila[6]);
            }
            catch (Exception)
            {
                nuevo = null;
            }
            return nuevo;
        }

    }
}