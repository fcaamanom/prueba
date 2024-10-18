using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Entidad
    {

        public int ID { get; set; }
        public string IDNacionalidad { get; set; }
        public string Nombre { get; set; }

        public static Entidad getData(DataRow fila)
        {
            Entidad nuevo = new Entidad();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.Nombre = Convert.ToString(fila[1]);
                nuevo.IDNacionalidad = Convert.ToString(fila[2]);
            }
            catch (Exception)
            {
                nuevo = null;
            }
            return nuevo;
        }

    }
}