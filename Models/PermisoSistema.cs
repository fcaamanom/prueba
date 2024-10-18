using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class PermisoSistema
    {
        public string Usuario { get; set; }
        public string Sistema { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public ObjetoPermiso[] Permisos { get; set; }
    }
}