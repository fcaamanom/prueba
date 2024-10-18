using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Plantilla
    {
        public int Cod_Tipo_Solicitud { get; set; }
        public int Cod_Condicion_Migratoria { get; set; }
        public int Cod_Condicion_Laboratorio { get; set; }
        public int Cod_Documento { get; set; }
        public string Naturaleza_Tramite { get; set; }

        //Franklin Caamaño
        //Estas columnas estan en el query, pero no son requeridas en la documentracion, por eso estan doumentadas, pero si se requieren solo hay que descomentarlas.
        //public string Doc_Nombre { get; set; }
        //public string Lab_Condicion_Nombre { get; set; }
        //public string Pla_Requerida { get; set; }
        //public string Migracion_Condicion_Nombre { get; set; }
        //public string Tipo_Solicitud_Nombre { get; set; }

        public static Plantilla getData(DataRow fila)
        {
            Plantilla nuevo = new Plantilla();
            try
            {
                nuevo.Cod_Tipo_Solicitud = Convert.ToInt32(fila[0]);
                nuevo.Cod_Condicion_Migratoria = Convert.ToInt32(fila[1]);
                nuevo.Cod_Condicion_Laboratorio = Convert.ToInt32(fila[2]);
                nuevo.Cod_Documento = Convert.ToInt32(fila[3]);
                nuevo.Naturaleza_Tramite = Convert.ToString(fila[4]);
            }
            catch (Exception)
            {
                nuevo = null;
            }
            return nuevo;
        }
    }
}