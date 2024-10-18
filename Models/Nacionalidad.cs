using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Nacionalidad
    {

        public string ID { get; set; }
        public string Nombre { get; set; }

        public static Nacionalidad getData(DataRow fila)
        {
            Nacionalidad nuevo = new Nacionalidad();
            try
            {
                nuevo.ID = Convert.ToString(fila[0]);
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