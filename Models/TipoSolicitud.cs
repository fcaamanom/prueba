using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class TipoSolicitud
    {

        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public static TipoSolicitud getData(DataRow fila)
        {
            TipoSolicitud nuevo = new TipoSolicitud();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.Nombre = Convert.ToString(fila[1]);
                nuevo.Descripcion = Convert.ToString(fila[2]);
            }
            catch (Exception)
            {
                nuevo = null;
            }

            return nuevo;
        }
    }
}