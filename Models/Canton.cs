using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Canton
    {

        public int ID { get; set; }
        public int IDProvincia { get; set; }
        public string Nombre { get; set; }

        public static Canton getData(DataRow fila)
        {
            Canton nuevo = new Canton();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.IDProvincia = Convert.ToInt32(fila[1]);
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