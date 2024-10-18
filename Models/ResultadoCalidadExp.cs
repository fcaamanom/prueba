using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class ResultadoCalidadExp
    {
        public int numeroExpediente { get; set; }
        public int puestoMigratorio { get; set; }
        public string  fechaExpediente { get; set; }
        public string estadoExpediente { get; set; }
        public int tipoExpediente { get; set; }
        public int puesto { get; set; }
        public int calidad { get; set; }
        public int tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public DateTime  fechaNacimiento { get; set; }
        public string nacionalidad { get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static ResultadoCalidadExp cargarFila(DataRow item)
        {
            ResultadoCalidadExp nuevo = new ResultadoCalidadExp();
            try
            {
                nuevo.numeroExpediente = Convert.ToInt32(item[0]);
                nuevo.puestoMigratorio = Convert.ToInt32(item[1]);
                nuevo.fechaExpediente = Convert.ToString(item[2]);
                nuevo.estadoExpediente = Convert.ToString(item[3]);
                nuevo.tipoExpediente = Convert.ToInt32(item[4]);
                nuevo.puesto = Convert.ToInt32(item[5]);
                nuevo.calidad = Convert.ToInt32(item[6]);
                nuevo.tipoIdentificacion = Convert.ToInt32(item[7]);
                nuevo.identificacion = Convert.ToString(item[8]);
                nuevo.fechaNacimiento = Convert.ToDateTime(item[9]);
                nuevo.nacionalidad = Convert.ToString(item[10]);
                nuevo.nombre = Convert.ToString(item[11]);
                nuevo.primerApellido = Convert.ToString(item[12]);
                nuevo.segundoApellido = Convert.ToString(item[13]);
            }
            catch (Exception msgEx)
            {
                EventLog.WriteEntry("ResultadoCalidadExp", msgEx.Message, EventLogEntryType.Error);
            }            
            return nuevo;
        }
    }
}