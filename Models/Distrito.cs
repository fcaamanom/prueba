using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Distrito
    {
        public int ID { get; set; }
        public int IDCanton { get; set; }
        public string Nombre { get; set; }

        public static Distrito getData(DataRow fila)
        {
            Distrito nuevo = new Distrito();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.IDCanton = Convert.ToInt32(fila[1]);
                nuevo.Nombre = Convert.ToString(fila[2]);
            }
            catch (Exception)
            {
                nuevo = null;
            }
            return nuevo;
        }
    }
}