namespace ApiExtranjeros.Models.Queries
{
    public class InformacionGeneral
    {
        public static Dictionary<int, string> Generos { get; private set; }
        public static Dictionary<string, string> EstadosCiviles { get; private set; }
        public static Dictionary<string, string> Escolaridades { get; private set; }
        public static Dictionary<string, string> RolesEntidad { get; private set; }
        public static Dictionary<string, string> TiposTramite { get; private set; }
        public static Dictionary<string, string> EstadosEstudios { get; private set; }
        public static Dictionary<string, string> TipoMonedas { get; private set; }

        static InformacionGeneral()
        {
            Generos = new List<Genero>
            {
                new Genero { ID = 0, Nombre = "Masculino" },
                new Genero { ID = 1, Nombre = "Femenino" },
                new Genero { ID = 2, Nombre = "Otro" }
            }.ToDictionary(g => g.ID, g => g.Nombre);

            EstadosCiviles = new List<EstadoCivil>
            {
                new EstadoCivil { ID = "C", Nombre = "CASADO(A)" },
                new EstadoCivil { ID = "D", Nombre = "DIVORCIADO(A)" },
                new EstadoCivil { ID = "S", Nombre = "SOLTERO(A)" },
                new EstadoCivil { ID = "V", Nombre = "VIUDO(A)" },
                new EstadoCivil { ID = "U", Nombre = "UNION LIBRE" }
            }.ToDictionary(e => e.ID, e => e.Nombre);

            Escolaridades = new List<Escolaridad>
            {
                new Escolaridad { ID = "N", Nombre = "NO INDICA" },
                new Escolaridad { ID = "P", Nombre = "PRIMARIA" },
                new Escolaridad { ID = "S", Nombre = "SECUNDARIA" },
                new Escolaridad { ID = "T", Nombre = "TECNICA" },
                new Escolaridad { ID = "U", Nombre = "UNIVERSITARIA" }
            }.ToDictionary(e => e.ID, e => e.Nombre);

            RolesEntidad = new List<RolEntidad>
            {
                new RolEntidad { Id = "E", Nombre = "EMPRESA" },
                new RolEntidad { Id = "P", Nombre = "PERSONAL" },
                new RolEntidad { Id = "A", Nombre = "APODERADO" },
                new RolEntidad { Id = "R", Nombre = "REPRESENTANTE LEGAL" },
                new RolEntidad { Id = "T", Nombre = "TERCERO" },
                new RolEntidad { Id = "F", Nombre = "FUNCIONARIO CORREGIR PLANTILLA" }
            }.ToDictionary(re => re.Id, re => re.Nombre);

            TiposTramite = new List<TipoTramite>
            {
                new TipoTramite { ID = "N", Nombre = "NUEVO" },
                new TipoTramite { ID = "R", Nombre = "RENOVACION" },
                new TipoTramite { ID = "D", Nombre = "DUPLICADO" },
                new TipoTramite { ID = "O", Nombre = "OTROS/NO APLICA" }
            }.ToDictionary(tt => tt.ID, tt => tt.Nombre);

            EstadosEstudios = new List<EstadoEstudio>
            {
                new EstadoEstudio { ID = "RF", Nombre = "RF" },
                new EstadoEstudio { ID = "RG", Nombre = "RG" },
                new EstadoEstudio { ID = "RI", Nombre = "RI" },
                new EstadoEstudio { ID = "RN", Nombre = "RN" },
                new EstadoEstudio { ID = "RP", Nombre = "RP" },
                new EstadoEstudio { ID = "RR", Nombre = "RR" },
                new EstadoEstudio { ID = "RT", Nombre = "RT" },
                new EstadoEstudio { ID = "RU", Nombre = "RU" },
                new EstadoEstudio { ID = "RV", Nombre = "RV" },
                new EstadoEstudio { ID = "SL", Nombre = "SL" },
                new EstadoEstudio { ID = "SU", Nombre = "SU" },
                new EstadoEstudio { ID = "UN", Nombre = "UN" },
                new EstadoEstudio { ID = "VO", Nombre = "VO" },
                new EstadoEstudio { ID = "XD", Nombre = "XD" },
                new EstadoEstudio { ID = "XI", Nombre = "XI" },
                new EstadoEstudio { ID = "XM", Nombre = "XM" },
                new EstadoEstudio { ID = "XP", Nombre = "XP" }

            }.ToDictionary(tt => tt.ID, tt => tt.Nombre);


            TipoMonedas = new List<TipoMoneda>
            {
                new TipoMoneda { ID = "COL", Nombre = "COL" },
                new TipoMoneda { ID = "DOL", Nombre = "DOL" }

            }.ToDictionary(tt => tt.ID, tt => tt.Nombre);
        }
    }
}
