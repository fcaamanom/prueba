using WSTramitaYa.Models;
using ApiExtranjeros.Common;
using ApiExtranjeros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ApiExtranjeros.Models.Queries;
using static ApiExtranjeros.Common.SybaseDBUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApiExtranjeros.Negocio;

namespace ApiExtranjeros.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly ApiSeguridad _apiSeguridad;
        private readonly Catalogo _catalogo;
        private readonly ConnectionStrings _connectionStrings;
        private string _methodName => ControllerContext.RouteData.Values["action"].ToString();
        private const string _NOPERMISOS = "No tiene permiso para ejecutar este método.";

        public CatalogoController(ApiSeguridad apiSeguridad, IOptions<Configuration> configuration, Catalogo catalogo)
        {
            _apiSeguridad = apiSeguridad;
            _catalogo = catalogo;
            _connectionStrings = configuration.Value.connectionStrings;
        }

        [HttpGet("ObtieneProvincia")]
        public async Task<IActionResult> getProvincia()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_provincia"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var provincias = General.ConsultaSQL<Provincia>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(provincias);
        }

        [HttpGet("ObtieneCanton")]
        public async Task<IActionResult> getCanton()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_canton"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var cantones = General.ConsultaSQL<Canton>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(cantones);
        }

        [HttpGet("ObtieneDistrito")]
        public async Task<IActionResult> getDistrito()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_distrito"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var distritos = General.ConsultaSQL<Distrito>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(distritos);

        }

        [HttpGet("ObtieneNacionalidad")]
        public async Task<IActionResult> getNacionalidad()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_nacionalidad"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var nacionalidades = General.ConsultaSQL<Nacionalidad>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(nacionalidades);
        }

        [HttpGet("ObtieneOcupacion")]
        public async Task<IActionResult> getOcupacion()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_ocupacion"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var ocupaciones = General.ConsultaSQL<Ocupacion>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(ocupaciones);
        }

        [HttpGet("ObtienePuestosMigratorio")]
        public async Task<IActionResult> getPuestoMigratorio()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_puesto"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var puestosMigratorios = General.ConsultaSQL<PuestoMigratorio>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(puestosMigratorios);
        }

        [HttpGet("ObtieneEntidad")]
        public async Task<IActionResult> getEntidad()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_entidad"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var entidades = General.ConsultaSQL<Entidad>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(entidades);
        }

        [HttpGet("ObtieneTipoIdentificacion")]
        public async Task<IActionResult> getTipoIdentificacion()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_identificacion"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var documentos = General.ConsultaSQL<Documento>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(documentos);
        }

        [HttpGet("ObtienePlantillas")]
        public async Task<IActionResult> getPlantillas()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_identificacion")) //TODO: Pendiente de colocar el menu correcto
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var plantillas = General.ConsultaSQL<Plantilla>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(plantillas);
        }

        [HttpGet("ObtieneCondicionMigratoria")]
        public async Task<IActionResult> getCondicionMigratoria()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_condicion"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            var condicionesMigratorias = _catalogo.getCatalogo<CondicionMigratoria>(_methodName, _connectionStrings.extranjeria, HttpContext);

            return Ok(condicionesMigratorias);
        }

        [HttpGet("ObtieneCondicionLaboral")]
        public async Task<IActionResult> getCondicionLaboral()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_condlaboral"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var condicionesLaborales = General.ConsultaSQL<CondicionLaboral>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(condicionesLaborales);
        }

        [HttpGet("ObtieneTipoSolicitud")]
        public async Task<IActionResult> getTipoSolicitud()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_tiposolicitud"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var tiposSolicitudes = General.ConsultaSQL<TipoSolicitud>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(tiposSolicitudes);
        }

        [HttpGet("ObtieneTipoResolucion")]
        public async Task<IActionResult> getTipoResolucion()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_tiporesolucion"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var tiposResolucion = General.ConsultaSQL<TipoResolucion>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(tiposResolucion);
        }

        [HttpGet("ObtieneRequisito")]
        public async Task<IActionResult> getRequisito()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_requisitos"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var requisitos = General.ConsultaSQL<Requisito>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(requisitos);
        }

        [HttpGet("ObtieneEmpresa")]
        public async Task<IActionResult> getEmpresa()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_empresa"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var empresas = General.ConsultaSQL<Empresa>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(empresas);
        }

        [HttpGet("ObtieneSexo")]
        public async Task<IActionResult> getSexo()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_sexo"))
            {
                return Unauthorized(_NOPERMISOS);
            }
            General.SybaseLog(HttpContext, tipoMensaje.Exito, "Consulta exitosa");
            return Ok(InformacionGeneral.Generos);
        }

        [HttpGet("ObtieneEstadoCivil")]
        public async Task<IActionResult> getEstadoCivil()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_estadocivil"))
            {
                return Unauthorized(_NOPERMISOS);
            }
            General.SybaseLog(HttpContext, tipoMensaje.Exito, "Consulta exitosa");
            return Ok(InformacionGeneral.EstadosCiviles);
        }

        [HttpGet("ObtieneEscolaridad")]
        public async Task<IActionResult> getEscolaridad()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_escolaridad"))
            {
                return Unauthorized(_NOPERMISOS);
            }
            General.SybaseLog(HttpContext, tipoMensaje.Exito, "Consulta exitosa");
            return Ok(InformacionGeneral.Escolaridades);
        }

        [HttpGet("ObtieneTipoSolicitante")]
        public async Task<IActionResult> getTipoSolicitante()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_tiposolicitante"))
            {
                return Unauthorized(_NOPERMISOS);
            }
            General.SybaseLog(HttpContext, tipoMensaje.Exito, "Consulta exitosa");
            return Ok(InformacionGeneral.RolesEntidad);
        }

        [HttpGet("ObtieneTramite")]
        public async Task<IActionResult> getTramite()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_tramite"))
            {
                return Unauthorized(_NOPERMISOS);
            }
            General.SybaseLog(HttpContext, tipoMensaje.Exito, "Consulta exitosa");
            return Ok(InformacionGeneral.TiposTramite);
        }

        [HttpGet("ObtienePronunciamientoVisas")]
        public async Task<IActionResult> getPronunciamientoVisas()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_pronunciamientovisas"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var pronunciamientos = General.ConsultaSQL<Pronunciamiento>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(pronunciamientos);
        }

        [HttpGet("ObtienePronunciamientoExtranjeria")]
        public async Task<IActionResult> getPronunciamientoExtranjeria()
        {
            if (await General.ValidarPermisoAsync(HttpContext, _apiSeguridad, "m_catalogo_pronunciamientoextranjeria"))
            {
                return Unauthorized(_NOPERMISOS);
            }

            string querySQL = CatalogoSqlQueries.GetQuery(_methodName);

            var pronunciamientos = General.ConsultaSQL<Pronunciamiento>(_connectionStrings.general, querySQL, HttpContext);

            return Ok(pronunciamientos);
        }

        [AllowAnonymous]
        [HttpGet("TesterQuery")]
        public async Task<IActionResult> getCondicionMigratoria(string querySQL)
        {
            var condicionesMigratorias = General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, querySQL, HttpContext);

            return Ok(condicionesMigratorias);
        }
    }
}
