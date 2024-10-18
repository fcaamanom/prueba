using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class TipoResolucion
    {

        public int ID { get; set; }
        public string Nombre { get; set; }

        public static TipoResolucion getData(DataRow fila)
        {
            TipoResolucion nuevo = new TipoResolucion();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.Nombre = Convert.ToString(fila[1]);
            }
            catch (Exception)
            {
                nuevo = null;
            }

            return nuevo;
        }
    }
}