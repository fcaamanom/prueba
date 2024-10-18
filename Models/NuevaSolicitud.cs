using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class NuevaSolicitud
    {

        public string codigoTramiteYa { get; set; }
        public string expedienteTramiteYa { get; set; }
        public string tipoSolicitudTramiteYa { get; set; }
        public int tipoExpediente { get; set; }
        public int numeroExpediente { get; set; }
        public int idPuesto { get; set; }
        public string formulario { get; set; }
        public string usuario { get; set; }
        public List<Documentos> documentos { get; set; }

    }
}