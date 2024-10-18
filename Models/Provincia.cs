using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Provincia
    {
        public int PROV_ID { get; set; }
        public string PROV_DESCRIPCION { get; set; }

        //public static Provincia getData(DataRow fila)
        //{
        //    Provincia nuevo = new Provincia();
        //    try
        //    {
        //        nuevo.PROV_ID = Convert.ToInt32(fila[0]);
        //        nuevo.Nombre = Convert.ToString(fila[1]);
        //    }
        //    catch (Exception)
        //    {
        //        nuevo = null;
        //    }
        //    return nuevo;
        //}
    }
}