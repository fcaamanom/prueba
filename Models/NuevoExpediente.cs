using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTramitaYa.Models
{
    public class NuevoExpediente
    {

        public string codigoTramiteYa { get; set; }
        public int tipoExpediente { get; set; }
        public char tieneCalidades { get; set; }
        public int idPuesto { get; set; }
        public int idCalidad { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string nacionalidad{ get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string conocidoComo { get; set; }
        public int profesion { get; set; }
        public string puesto { get; set; }
        public bool genero { get; set; }
        public string cambioGenero { get; set; }
        public string lugarNacimiento { get; set; }
        public string estadoCivil { get; set; }
        public string nivelAcademico { get; set; }
        public int tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string tipoPasaporte { get; set; }
        public string numeroPasaporte { get; set; }
        public string dimex { get; set; }
        public char consentimiento { get; set; }
        public string nombrePadre { get; set; }
        public string primerApellidoPadre { get; set; }
        public string segundoApellidoPadre { get; set; }
        public string nombreMadre { get; set; }
        public string primerApellidoMadre { get; set; }
        public string segundoApellidoMadre { get; set; }
        public bool tieneIncapacidad { get; set; }
        public bool menorEdad { get; set; }
        public string curador { get; set; }
        public string patriaPotestad { get; set; }
        public string representanteLegal { get; set; }
        public DateTime fechaSalidaPaisOrigen { get; set; }
        public DateTime fechaIngresoCR { get; set; }
        public string puntoIngresoCR { get; set; }
        public int puestoIngreso { get; set; }
        public string telefonoHabitacion { get; set; }
        public string telefonoCelular { get; set; }
        public string telefonoOficina { get; set; }
        public string fax { get; set; }
        public string correo { get; set; }
        public string correoAdicional { get; set; }
        public string funcionario { get; set; }
        public int provincia { get; set; }
        public int canton { get; set; }
        public int distrito { get; set; }
        public string direccion { get; set; }
        public char tipoDireccion { get; set; }
        public int calDonador { get; set; }
    }
}