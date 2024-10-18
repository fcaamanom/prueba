using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Empresa
    {

        public int ID { get; set; }
        public string CedulaJuridica { get; set; }
        public string Nombre { get; set; }
        public string Alias { get; set; }
        public string GiroComercial { get; set; }
        public string IdRepresentanteLegal { get; set; }
        public string NombreRepresentanteLegal { get; set; }
        public string Categoria { get; set; }
        public string FechaRenovacion { get; set; }
        
        
        public static Empresa getData(DataRow fila)
        {
            Empresa nuevo = new Empresa();
            try
            {
                nuevo.ID = Convert.ToInt32(fila[0]);
                nuevo.CedulaJuridica = Convert.ToString(fila[1]);
                nuevo.Nombre = Convert.ToString(fila[2]);
                nuevo.Alias = Convert.ToString(fila[3]);
                nuevo.GiroComercial = Convert.ToString(fila[4]);
                nuevo.IdRepresentanteLegal = Convert.ToString(fila[5]);
                nuevo.NombreRepresentanteLegal = Convert.ToString(fila[6]);
                nuevo.Categoria = Convert.ToString(fila[7]);
                nuevo.FechaRenovacion = Convert.ToString(fila[8]);
            }
            catch (Exception)
            {
                nuevo = null;
            }
            return nuevo;
        }
    }
}