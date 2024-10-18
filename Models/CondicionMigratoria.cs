using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class CondicionMigratoria
    {

        public int ID { get; set; }
        public string Nombre { get; set; }

        public static CondicionMigratoria getData(DataRow fila)
        {
            CondicionMigratoria nuevo = new CondicionMigratoria();
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