using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class Documentos
    {
        public string documentos { get; set; }
        public string codigo { get; set; }
        public string url { get; set; }
        public string[] adjuntos { get; set; }
    }
}