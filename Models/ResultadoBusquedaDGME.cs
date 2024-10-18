using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class ResultadoBusquedaDGME
    {

        public ResultadoBusquedaDGME()
        {
            resultados = new List<ResultadoCalidadExp>();
        }

        public string codigoTramiteYa { get; set; }
        public int estado { get; set; }

        public List<ResultadoCalidadExp> resultados { get; set; }

    }
}