using ApiExtranjeros.Common;
using ApiExtranjeros.Models;
using ApiExtranjeros.Models.Busquedas;
using ApiExtranjeros.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WSTramitaYa.Models;

namespace ApiExtranjeros.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class BusquedaController(IOptions<Configuration> configuration) : ControllerBase
    {
        private readonly ConnectionStrings _connectionStrings = configuration.Value.connectionStrings;
        private string _methodName => ControllerContext.RouteData.Values["action"].ToString();

        [HttpPost("BuscarEmpresa")]
        public async Task<IActionResult> BuscarEmpresa([FromBody] EmpresaBusquedaRequest request)
        {
            if (string.IsNullOrEmpty(request.Funcionario))
            {
                return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "El funcionario es obligatorio." });
            }

            try
            {
                // Construir la consulta SQL según el tipo de búsqueda
                string consultaBase = CatalogoSqlQueries.GetQuery(_methodName);
                string consultaFinal = consultaBase;
                // Ejecutar la consulta SQL
                var result = await BuildQueryWhere(request, consultaBase);
                if (result is OkObjectResult okResult)
                {
                    var data = okResult.Value;
                    consultaFinal = data.ToString();
                }

                var parametros = new
                {
                    CedulaJuridica = request.CedulaJuridica,
                    NombreEmpresa = "%" + request.NombreEmpresa + "%",
                    AliasEmpresa = "%" + request.AliasEmpresa + "%"
                };

                var resultados = General.ConsultaSQLConParametros<EmpresaBusquedaResponse>(_connectionStrings.extranjeria, consultaFinal, parametros, HttpContext);

                if (resultados == null || !resultados.Any())
                {
                    return NotFound(new ErrorResponse { Codigo = 404, Descripcion = "No se encontraron resultados." });
                }

                return Ok(new { resultados });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Codigo = 500, Descripcion = "Error interno del servidor: " + ex.Message });
            }
        }
        #region Metodo  (BuildQueryWhere) ==>  Construir Query Empresas
        //Construir Query
        private async Task<IActionResult> BuildQueryWhere(EmpresaBusquedaRequest request, string consultaBase)
        {
            string whereClause = "";

            // Modificar la consulta según el tipo de búsqueda
            switch (request.TipoBusqueda)
            {
                case 0: // Full búsqueda
                    if (string.IsNullOrEmpty(request.CedulaJuridica) || string.IsNullOrEmpty(request.NombreEmpresa) || string.IsNullOrEmpty(request.AliasEmpresa))
                    {
                        return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Todos los parámetros son requeridos para la búsqueda tipo 0." });
                    }
                    whereClause = "AND E.EMP_CEDULA_JURIDICA = @CedulaJuridica AND E.EMP_NOMBRE LIKE @NombreEmpresa AND E.EMP_ALIAS LIKE @AliasEmpresa";
                    break;
                case 1: // Búsqueda por cédula jurídica
                    if (string.IsNullOrEmpty(request.CedulaJuridica))
                    {
                        return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Cédula jurídica es requerida para la búsqueda tipo 1." });
                    }
                    whereClause = "AND E.EMP_CEDULA_JURIDICA = @CedulaJuridica";
                    break;
                case 2: // Búsqueda por nombre de empresa
                    if (string.IsNullOrEmpty(request.NombreEmpresa))
                    {
                        return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "El nombre de la empresa es requerido para la búsqueda tipo 2." });
                    }
                    whereClause = "AND E.EMP_NOMBRE LIKE @NombreEmpresa";
                    break;
                case 3: // Búsqueda por alias
                    if (string.IsNullOrEmpty(request.AliasEmpresa))
                    {
                        return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "El alias de la empresa es requerido para la búsqueda tipo 3." });
                    }
                    whereClause = "AND E.EMP_ALIAS LIKE @AliasEmpresa";
                    break;
                default:
                    return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Tipo de búsqueda inválido." });
            }
            return Ok(consultaBase + whereClause + "    ORDER BY C.EMP_CAT_FECHA_RENOVACION DESC");
        }

        #endregion

        [HttpPost("BuscarDocumentoUnico")]
        public async Task<IActionResult> BuscarDocumentoUnico([FromBody] DocumentoUnicoBusquedaRequest request)
        {
            // Validar los parámetros de entrada
            if (string.IsNullOrEmpty(request.NumeroDocUnico) || string.IsNullOrEmpty(request.Funcionario))
            {
                return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "El número de documento único y el funcionario son obligatorios." });
            }

            try
            {
                // Definir la consulta SQL
                string consultaSQL = CatalogoSqlQueries.GetQuery(_methodName);

                var documentoUnico = General.ConsultaSQLConParametros<DocumentoUnicoBusquedaResponse>(_connectionStrings.extranjeria, consultaSQL, new
                {
                    NumeroDocUnico = request.NumeroDocUnico
                }, HttpContext).FirstOrDefault();

                if (documentoUnico == null)
                {
                    return NotFound(new ErrorResponse { Codigo = 404, Descripcion = "No se encontraron resultados." });
                }
                //Agregar detalle de nacionalidad en vez del ID
                GetNacionalidadById(documentoUnico);

                return Ok(documentoUnico);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Codigo = 500, Descripcion = "Error interno del servidor: " + ex.Message });
            }
        }

        #region Metodo  (GetNacionalidadById) ==>  Traer Nombre de Nacionalidad desde otra base de datos
        private async void GetNacionalidadById(DocumentoUnicoBusquedaResponse documentoUnico)
        {
            //Franklin: Obtener Nacionalidad, ya que en la conexion de extranjeria no se verifica este catalogo
            if (!string.IsNullOrEmpty(documentoUnico.Nacionalidad))
            {
                string querySQL = CatalogoSqlQueries.GetQuery("getNacionalidad");
                var nacionalidades = General.ConsultaSQL<Nacionalidad>(_connectionStrings.general, querySQL, HttpContext);
                if (nacionalidades != null)
                {
                    var data = nacionalidades.Where(x => x.ID.Trim() == documentoUnico.Nacionalidad.Trim()).ToList();
                    if (data.Any())
                        documentoUnico.Nacionalidad = data.FirstOrDefault().Nombre;
                }

            }

        }
        #endregion


        /// <summary>
        /// Desarrollado por: Novacomp,
        /// Fecha de Creacion: 06/09/2024
        /// Descripcion: Todos los filtros son requeridos 
        /// </summary>
        /// <param name="request">Filtros: 
        ///     {numeroDeposito, Requerido, maximumLength: 200},
        ///     {fechaDeposito, Requerido datetime}, 
        ///     {Funcionario, Requerido, maximumLength: 30}
        /// </param>
        /// <returns></returns> 

        [HttpPost("BuscarDeposito")]
        public async Task<IActionResult> BuscarDeposito([FromBody] DepositoBusquedaRequest request)
        {

            // Validar los parámetros de entrada
            if (string.IsNullOrEmpty(request.numeroDeposito) || string.IsNullOrEmpty(request.Funcionario) || request.fechaDeposito == null)
                return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Error incorrecto en los parámetros." });

            // Validar longitud de campos
            if (request.numeroDeposito.Length > 200 || request.Funcionario.Length > 30)
                return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Error incorrecto en longitud de los parámetros." });

            try
            {

                // Definir la consulta SQL
                string consultaSQL = CatalogoSqlQueries.GetQuery(_methodName);
                var parametros = new
                {
                    numeroDeposito = request.numeroDeposito,
                    Funcionario = request.Funcionario,
                    fechadeposito = request.fechaDeposito.ToString("yyyy-MM-dd")
                };

                //Trae un solo registro en vez de traer un listado de registros
                var deposito = General.ConsultaSQLConParametros<DepositoBusquedaResponse>(_connectionStrings.financiero, consultaSQL, parametros, HttpContext).FirstOrDefault();

                if (deposito == null)
                {
                    return NotFound(new ErrorResponse { Codigo = 404, Descripcion = "No se encontraron resultados." });
                }

                return Ok(deposito);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Codigo = 500, Descripcion = "Error interno del servidor: " + ex.Message });
            }
        }

        [HttpPost("BuscarDGME")]
        public async Task<IActionResult> BuscarDGME([FromBody] DGMERequest request)
        {

            try
            {
                // Construir la consulta SQL según el tipo de búsqueda
                string consultaBase = "";

                // Ejecutar la consulta SQL
                var result = new ParametrosConsultas();
                var dataConsulta = await BuildDGMEQueryAndParams(request, _methodName);
                if (dataConsulta is OkObjectResult okResult)
                {
                    var data = okResult.Value;
                    result = (ParametrosConsultas)data;
                }


                var resultados = General.ConsultaSQLConParametros<DGMEResponse>(_connectionStrings.extranjeria, result.Consulta, result.Parametros, HttpContext);

                if (resultados == null || !resultados.Any())
                {
                    return NotFound(new ErrorResponse { Codigo = 404, Descripcion = "No se encontraron resultados." });
                }

                return Ok(new { resultados });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Codigo = 500, Descripcion = "Error interno del servidor: " + ex.Message });
            }
        }
        #region Metodo  (BuildDGMEQueryAndParams) ==>  Construir Query DGME
        //Construir Query
        private async Task<IActionResult> BuildDGMEQueryAndParams(DGMERequest request, string _methodNameCurrent)
        {

            var result = new ParametrosConsultas();

            // Modificar la consulta según el tipo de búsqueda
            switch (request.TipoBusqueda)
            {
                case 0: // Full búsqueda
                    {
                        if (string.IsNullOrEmpty(request.NumeroExpediente) || request.PuestoMigratorio is null || request.TipoIdentificacion is null || string.IsNullOrEmpty(request.Identificacion) || request.FechaNacimiento is null || string.IsNullOrEmpty(request.Nacionalidad) || string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.PrimerApellido) || string.IsNullOrEmpty(request.SegundoApellido))
                        {
                            return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Todos los parámetros son requeridos para la búsqueda tipo 0." });
                        }
                        result.Consulta = CatalogoSqlQueries.GetQuery(_methodNameCurrent + "/Full");
                        result.Parametros = new
                        {
                            NumeroExpediente = request.NumeroExpediente,
                            PuestoMigratorio = request.PuestoMigratorio,
                            TipoIdentificacion = request.TipoIdentificacion,
                            Identificacion = request.Identificacion,
                            FechaNacimiento = request.FechaNacimiento,
                            Nacionalidad = request.Nacionalidad,
                            Nombre = request.Nombre,
                            PrimerApellido = request.PrimerApellido,
                            SegundoApellido = request.SegundoApellido,
                        };
                    }

                    break;
                case 1: // Búsqueda por número de expediente
                    {
                        if (string.IsNullOrEmpty(request.NumeroExpediente))
                        {
                            return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Número de Expediente es requerida para la búsqueda tipo 1." });
                        }
                        result.Consulta = CatalogoSqlQueries.GetQuery(_methodNameCurrent + "/Expediente");
                        result.Parametros = new
                        {
                            NumeroExpediente = request.NumeroExpediente,
                        };
                    }
                    break;
                case 2: // Búsqueda por número de documento
                    {
                        if (request.TipoIdentificacion is null || string.IsNullOrEmpty(request.Identificacion))
                        {
                            return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "El tipo de identificación y número de identificación son requeridos para la búsqueda tipo 2." });
                        }
                        result.Consulta = CatalogoSqlQueries.GetQuery(_methodNameCurrent + "/Identificacion");
                        result.Parametros = new
                        {
                            TipoIdentificacion = request.TipoIdentificacion,
                            Identificacion = request.Identificacion,
                            FechaNacimiento = request.FechaNacimiento,

                        };
                    }
                    break;
                case 3: // Búsqueda apellidos y nombre
                    {
                        if (string.IsNullOrEmpty(request.Nacionalidad) || string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.PrimerApellido) || request.SegundoApellido is null)
                        {
                            return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "La nacionalidad, nombre, primer apellido y segundo apellido son requeridos para la búsqueda tipo 3." });
                        }
                        result.Consulta = CatalogoSqlQueries.GetQuery(_methodNameCurrent + "/ApellidoYNombre");
                        result.Parametros = new
                        {
                            Nacionalidad = request.Nacionalidad,
                            Nombre = request.Nombre,
                            PrimerApellido = request.PrimerApellido,
                            SegundoApellido = request.SegundoApellido,
                        };
                    }
                    break;
                case 4: // Búsqueda por like apellidos y nombre
                    {
                        if (string.IsNullOrEmpty(request.Nacionalidad) || string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.PrimerApellido) || request.SegundoApellido is null)
                        {
                            return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "La nacionalidad, nombre, primer apellido y segundo apellido son requeridos para la búsqueda tipo 4." });
                        }
                        result.Consulta = CatalogoSqlQueries.GetQuery(_methodNameCurrent + "/ApellidoYNombreLike");
                        result.Parametros = new
                        {
                            Nacionalidad = request.Nacionalidad,
                            Nombre = "%" + request.Nombre + "%",
                            PrimerApellido = "%" + request.PrimerApellido + "%",
                            SegundoApellido = "%" + request.SegundoApellido + "%",
                        };

                    }

                    break;
                default:
                    return BadRequest(new ErrorResponse { Codigo = 400, Descripcion = "Tipo de búsqueda inválido." });
            }
            return Ok(result);
        }


        #endregion

        [HttpPost("ObtenerConsecutivo")]
        public async Task<IActionResult> ObtenerConsecutivo([FromBody] ConsecutivoRequest request)
        {
            // Simulación de la lógica para obtener el consecutivo
            var resultado = await ObtenerConsecutivoAsync(request);

            if (resultado != null)
            {
                return Ok(resultado);
            }

            return BadRequest("Error al obtener el consecutivo.");
        }

        private async Task<ConsecutivoResponse> ObtenerConsecutivoAsync(ConsecutivoRequest request)
        {
            // lógica

            await Task.Delay(100); // Simulación de operación asíncrona

            // Simulando la respuesta
            return new ConsecutivoResponse
            {
                Consecutivo = 1234567890,
                PuestoConsecutivo = request.IdPuestoMigratorio ?? 0,
                Nacionalidad = request.Nacionalidad ?? "N/A"
            };
        }


        [HttpPost("ActualizarConsecutivo")]
        public async Task<IActionResult> ActualizarConsecutivo([FromBody] ConsecutivoRequest request)
        {
            // Simulación de la lógica para actualizar el consecutivo
            var resultado = await ActualizarConsecutivoAsync(request);

            if (resultado != null)
            {
                return Ok(new
                {
                    Consecutivo = resultado.Consecutivo,
                    puestoConsecutivo = resultado.PuestoConsecutivo,
                    Nacionalidad = resultado.Nacionalidad
                });
            }
            return BadRequest("Error al actualizar el consecutivo.");
        }

        private async Task<ConsecutivoResponse> ActualizarConsecutivoAsync(ConsecutivoRequest request)
        {
            // Aquí iría la lógica para actualizar el consecutivo en la base de datos.

            await Task.Delay(100); // Simulación de operación asíncrona

            // Simulando la actualización exitosa y la respuesta
            return new ConsecutivoResponse
            {
                Consecutivo = request.ConsecutivoNuevo,
                PuestoConsecutivo = request.IdPuestoMigratorio ?? 0,
                Nacionalidad = request.Nacionalidad ?? "N/A"
            };
        }

        [HttpPost("ObtenerPlantillaRequisito")]
        public async Task<IActionResult> ObtenerPlantillaRequisitoAsync([FromBody] PlantillaRequisitoRequest request)
        {
            // Definir la consulta SQL
            string consultaSQL = CatalogoSqlQueries.GetQuery(_methodName);

            var resultado = General.ConsultaSQLConParametros<PlantillaRequisitoResponse>(
                _connectionStrings.extranjeria,
                consultaSQL, request,
                HttpContext).ToList();

            if (resultado is not null)
            {
                return Ok(resultado);

            }

            return NotFound(new ErrorResponse { Codigo = 404, Descripcion = "No se encontraron resultados." });
        }
    }
}
