using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class MOD_SISTEMA
    {

        public string PREF_ID { get; set; }
        public string USUARIO { get; set; }
        public string Log_Usuario { get; set; }
        public string Log_Sistema { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public List<MOD_MENU> menu { get; set; }

    }
}