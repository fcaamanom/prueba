using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Requisito
    {

        public int ID { get; set; }
        public string Nombre { get; set; }
        public string TipoRequisito { get; set; }
        public int PlazoDias { get; set; }

        public static Requisito getData(DataRow fila)
        {
            Requisito nuevo = new Requisito();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.Nombre = Convert.ToString(fila[1]);
                nuevo.TipoRequisito = Convert.ToString(fila[2]);
                nuevo.PlazoDias = Convert.ToInt32(fila[3]);
            }
            catch (Exception)
            {
                nuevo = null;
            }

            return nuevo;
        }
    }
}