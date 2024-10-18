using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Dimex
    {

        public string NDimex { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }

        public string Categoria { get; set; }
        public string SubCategoria { get; set; }

        public static Dimex cargarDimex(DataRow fila)
        {
            Dimex nuevo = new Dimex();
            try
            {
                nuevo.NDimex = Convert.ToString(fila[0]);
                nuevo.Nombre = Convert.ToString(fila[3]);
                nuevo.Apellido1 = Convert.ToString(fila[1]);
                nuevo.Apellido2 = Convert.ToString(fila[2]);

                nuevo.Categoria = Convert.ToString(fila[4]);
                nuevo.SubCategoria = Convert.ToString(fila[5]);
            }
            catch (Exception)
            {
                nuevo = null;
            }
            return nuevo;
        }

    }
}