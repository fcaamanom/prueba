namespace ApiExtranjeros.Models.Expediente
{
    public class CrearResolucionRequest
    {
        // Parámetros requeridos
        public int IdPuestoCalidad { get; set; }
        public long NumeroCalidad { get; set; }
        public int IdPuestoSolicitud { get; set; }
        public long NumeroSolicitud { get; set; }
        public int Identificador { get; set; } 
        public short Pronunciamiento { get; set; }
        public int IdFirma { get; set; }
        public int MayoriaEdad { get; set; } 
        public string Funcionario { get; set; }

        // Parámetros opcionales
        public int? IdPuestoEstudio { get; set; }
        public long? IdEstudioTecnico { get; set; }
        public string Vinculo { get; set; }
        public string Motivo { get; set; }
        public string ComentarioGeneral { get; set; }

        // Archivo PDF
        public IFormFile ResolucionPdf { get; set; }
    }

}
