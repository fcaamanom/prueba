using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{

    public class BusquedaDGME
    {

        public string codigoTramiteYa { get; set; }
        public int tipoBusqueda { get; set; }
        public int numeroExpediente { get; set; }
        public int puestoMigratorio { get; set; }
        public int tipoExpediente { get; set; }
        public int tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public DateTime  fechaNacimiento { get; set; }
        public string nacionalidad { get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }


    }
}