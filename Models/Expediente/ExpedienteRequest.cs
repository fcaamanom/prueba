namespace ApiExtranjeros.Models.Expediente
{
    public class ExpedienteRequest
    {
        public int? IdPuesto { get; set; }
        public int CondicionMigratoria { get; set; }
        public int CondicionLaboral { get; set; }
        public int TipoSolicitud { get; set; }
        public string PlanReqNaturaleza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? ConocidoComo { get; set; }
        public int Profesion { get; set; }
        public int Sexo { get; set; }
        public string? CambioGenero { get; set; }
        public string EstadoCivil { get; set; }
        public string NivelAcademico { get; set; }
        public string LugarNacimiento { get; set; }
        public int TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? DocumentoUnico { get; set; }
        public string Consentimiento { get; set; }
        public string NombrePadre { get; set; }
        public string PrimerApellidoPadre { get; set; }
        public string SegundoApellidoPadre { get; set; } = string.Empty;
        public string NombreMadre { get; set; }
        public string PrimerApellidoMadre { get; set; }
        public string SegundoApellidoMadre { get; set; } = string.Empty;
        public int DonaOrganos { get; set; }
        public DateTime? FechaDonacion { get; set; }
        public int TieneIncapacidad { get; set; }
        public bool MenorEdad { get; set; }
        public string? PatriaPotestad { get; set; }
        public string? RepresentanteLegal { get; set; }
        public DateTime? FechaSalidaPaisOrigen { get; set; }
        public DateTime? FechaIngresoCR { get; set; }
        public int? PuntoIngresoCR { get; set; }
        public string? TelefonoHabitacion { get; set; }
        public string? TelefonoCelular { get; set; }
        public string? TelefonoTrabajo { get; set; }
        public string? Fax { get; set; }
        public string Correo { get; set; }
        public int Provincia { get; set; }
        public int Canton { get; set; }
        public int Distrito { get; set; }
        public string? Direccion { get; set; }
        public string TipoDireccion { get; set; }

        public List<PlantillaRequisito> plantillaRequisitos { get; set; }
    }
}


public class PlantillaRequisito
{
    public int DocId { get; set; }
    public string Registrado { get; set; }
    public string UrlDoc { get; set; }
}