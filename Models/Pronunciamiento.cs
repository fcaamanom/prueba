using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Pronunciamiento
    {

        public int Resolucion { get; set; }
        public string Estudio { get; set; }
        public string Descripcion { get; set; }

        public static Pronunciamiento getData(DataRow fila)
        {
            Pronunciamiento nuevo = new Pronunciamiento();
            try
            {
                nuevo.Resolucion = Convert.ToInt32(fila[0]);
                nuevo.Estudio = Convert.ToString(fila[1]);
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