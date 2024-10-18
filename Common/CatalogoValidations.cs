
using ApiExtranjeros.Models;
using ApiExtranjeros.Models.Busquedas;
using ApiExtranjeros.Models.Queries;
using ApiExtranjeros.Negocio;
using Microsoft.Extensions.Options;
using WSTramitaYa.Models;

namespace ApiExtranjeros.Common
{
    public class CatalogoValidations
    {
        private readonly Catalogo _catalogo;
        private readonly ConnectionStrings _connectionStrings;

        public CatalogoValidations(Catalogo catalogo, IOptions<Configuration> configuration)
        {
            _catalogo = catalogo;
            _connectionStrings = configuration.Value.connectionStrings;
        }

        public bool BeAValidCondicionMigratoria(int condicionMigratoriaId)
        {
            var condicion = _catalogo.getCatalogo<CondicionMigratoria>("getCondicionMigratoria", _connectionStrings.extranjeria, null)
                                      .FirstOrDefault(c => c.ID == condicionMigratoriaId);
            return condicion != null;
        }

        public bool BeAValidCondicionLaboral(int condicionLaboralId)
        {
            var result = _catalogo.getCatalogo<CondicionLaboral>("getCondicionLaboral", _connectionStrings.extranjeria, null)
                                  .FirstOrDefault(c => c.ID == condicionLaboralId);
            return result != null;
        }

        public bool BeAValidTipoSolicitud(int tipoSolicitudId)
        {
            var result = _catalogo.getCatalogo<TipoSolicitud>("getTipoSolicitud", _connectionStrings.extranjeria, null)
                                  .FirstOrDefault(c => c.ID == tipoSolicitudId);
            return result != null;
        }

        public bool BeAValidNacionalidad(string nacionalidadId)
        {
            var result = _catalogo.getCatalogo<Nacionalidad>("getNacionalidad", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.ID.Trim().Equals(nacionalidadId.Trim(), StringComparison.OrdinalIgnoreCase));
            return result != null;
        }

        public bool BeAValidOcupacion(int ocupacionId)
        {
            var result = _catalogo.getCatalogo<Ocupacion>("getOcupacion", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.ID == ocupacionId);
            return result != null;
        }

        public bool BeAValidTipoDocumento(int tipoDocId)
        {
            var result = _catalogo.getCatalogo<Documento>("getTipoIdentificacion", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.ID == tipoDocId);
            return result != null;
        }

        public bool BeAValidProvincia(int provinciaId)
        {
            var result = _catalogo.getCatalogo<Provincia>("getProvincia", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.PROV_ID == provinciaId);
            return result != null;
        }

        public bool BeAValidCanton(int cantonId, int provinciaId)
        {
            var result = _catalogo.getCatalogo<Canton>("getCanton", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.ID == cantonId && c.IDProvincia == provinciaId);
            return result != null;
        }

        public bool BeAValidDistrito(int distritoId, int cantonId)
        {
            var result = _catalogo.getCatalogo<Distrito>("getDistrito", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.ID == distritoId && c.IDCanton == cantonId);
            return result != null;
        }

        public bool BeAValidPuestoMigratorio(int? puestoId)
        {
            var result = _catalogo.getCatalogo<PuestoMigratorio>("getPuestoMigratorio", _connectionStrings.general, null)
                                  .FirstOrDefault(c => c.ID == puestoId);
            return result != null;
        }

        public bool BeAValidEstadoCivil(string estadoCivil)
        {
            var result = InformacionGeneral.EstadosCiviles.FirstOrDefault(c => c.Key == estadoCivil.ToUpper()).Value;
            return result != null;
        }

        public bool BeAValidNivelAcademico(string nivelAcademico)
        {
            var result = InformacionGeneral.Escolaridades.FirstOrDefault(c => c.Key == nivelAcademico.ToUpper()).Value;
            return result != null;
        }

        public List<PlantillaRequisitoResponse> GetPlantillasTodas(int tipoSolicId, int condMigratoriaId, int condLaboralId, string natuTramite)
        {
            var parametros = new PlantillaRequisitoRequest()
            {
                IdTipoSolicitud = tipoSolicId,
                IdCondicionMigratoria = condMigratoriaId,
                IdCondicionLaboral = condLaboralId,
                NaturalezaTramite = natuTramite
            };

            return _catalogo.Buscar<PlantillaRequisitoResponse>("ObtenerPlantillaRequisito", parametros, null).ToList();
        }

        public bool BeAValidCalidad(int calidad)
        {
            var querySQL = CatalogoSqlQueries.GetQuery("BuscarCalidadXId");

            // Realiza la consulta
            var result = General.ConsultaSQLConParametros<int>(
                _connectionStrings.extranjeria,
                querySQL,
                new { calidadId = calidad },
                null
            ).FirstOrDefault();

            // Verifica si se encontró algún resultado
            return result != 0;
        }

        public bool BeAValidSolicitud(int numSolicitud)
        {
            var querySQL = CatalogoSqlQueries.GetQuery("BuscarSolicitud");

            // Realiza la consulta
            var result = General.ConsultaSQLConParametros<int>(
                _connectionStrings.extranjeria,
                querySQL,
                new { NumeroSolicitud = numSolicitud },
                null
            ).FirstOrDefault();

            // Verifica si se encontró algún resultado
            return result != 0;
        }

        public bool BeAValidResolucion(int resolucion)
        {
            var querySQL = CatalogoSqlQueries.GetQuery("getTipoResolucion");

            var result = General.ConsultaSQL<TipoResolucion>(_connectionStrings.extranjeria, querySQL, null).FirstOrDefault(c => c.ID == resolucion);
            return result != null;
        }

        public bool BeAValidEstadoEstudio(string estadoEstudio)
        {
            var result = InformacionGeneral.EstadosEstudios.FirstOrDefault(c => c.Key == estadoEstudio.ToUpper()).Value;
            return result != null;
        }
        public string GetEstadoEstudio()
        {
            var result = string.Join(", ", InformacionGeneral.EstadosEstudios.Select(e => e.Value).ToArray());
            return result;
        }

        public bool BeAValidTipoMonedas(string estadoEstudio)
        {
            var result = InformacionGeneral.TipoMonedas.FirstOrDefault(c => c.Key == estadoEstudio.ToUpper()).Value;
            return result != null;
        }
        public string GetTipoMonedas()
        {
            var result = string.Join(", ", InformacionGeneral.TipoMonedas.Select(e => e.Value).ToArray());
            return result;
        }

    }
}
