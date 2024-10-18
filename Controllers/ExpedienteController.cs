using ApiExtranjeros.Common;
using ApiExtranjeros.Models;
using ApiExtranjeros.Models.Busquedas;
using ApiExtranjeros.Models.Expediente;
using ApiExtranjeros.Models.Queries;
using ApiExtranjeros.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.Data;

namespace ApiExtranjeros.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ExpedienteController(IOptions<Configuration> configuration, Catalogo _catalogo) : ControllerBase
    {
        private readonly ConnectionStrings _connectionStrings = configuration.Value.connectionStrings;
        private string _methodName => ControllerContext.RouteData.Values["action"].ToString();

        private readonly int idPuesto = 135; //pregunta?


        [HttpPost("CrearExpediente")]
        public async Task<IActionResult> CrearExpediente([FromBody] ExpedienteRequest request)
        {
            var nombreProcedimiento = "[SP_GET_ESCENARIOS]";
            var IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

            var funcionario = General.ObtenerUsuarioId(HttpContext);

            #region lógica para obtene la calidad

            var querySQL = CatalogoSqlQueries.GetQuery("BuscarDGME/NuevoExpediente");
            var parametros = new
            {
                Nacionalidad = request.Nacionalidad,
                Nombre = "%" + request.Nombre + "%",
                PrimerApellido = "%" + request.PrimerApellido + "%",
                SegundoApellido = "%" + request.SegundoApellido + "%", //agregar la fecha de nacimiento para la búsqueda
                FechaNacimiento = request.FechaNacimiento
            };


            var plantillaReqRequest = new PlantillaRequisitoRequest()
            {
                IdTipoSolicitud = request.TipoSolicitud,
                IdCondicionMigratoria = request.CondicionLaboral,
                IdCondicionLaboral = request.CondicionLaboral,
                NaturalezaTramite = request.PlanReqNaturaleza,
            };

            var listRequisitosPlantilla = _catalogo.Buscar<PlantillaRequisitoResponse>("ObtenerPlantillaRequisito", plantillaReqRequest, null).ToList();



            foreach (var item in listRequisitosPlantilla)
            {
                // Verifica si ya existe un elemento con el mismo DocId en la lista
                bool existe = request.plantillaRequisitos.Any(req => req.DocId == item.IdDocumento);

                // Si no existe, se añade un nuevo requisito
                if (!existe)
                {
                    request.plantillaRequisitos.Add(new PlantillaRequisito()
                    {
                        DocId = item.IdDocumento,
                        Registrado = "N",
                        UrlDoc = string.Empty
                    });
                }
            }



            // Llamada al método actualizado
            //InsertarRequisitosFaltantes(copia, listRequisitosPlantilla);

            var DGMEResponse = General.ConsultaSQLConParametros<DGMEResponse>(_connectionStrings.extranjeria, querySQL, parametros, HttpContext)
                                                                             .OrderByDescending(result => result.FechaExpediente).FirstOrDefault();

            #endregion
            var escenario = new Escenario();

            if (DGMEResponse is not null)
            {
                var parametrosEscenarios = new
                {
                    idPuesto = request.IdPuesto, // Valor por defecto
                    idCalidad = DGMEResponse.Calidad,
                    tipoSolicitud = request.TipoSolicitud,
                    condicionMigratoria = request.CondicionMigratoria,
                    condicionLaboral = request.CondicionLaboral
                };


                // Ejecuta el procedimiento para obtener el escenario
                escenario = General.ConsultaSQLServer<Escenario>(
                     _connectionStrings.SQL_18_PRUEBAS,
                     nombreProcedimiento,
                     HttpContext,
                     parametrosEscenarios
                 ).FirstOrDefault();

                if (escenario == null)
                {
                    return NotFound("No se encontró el escenario.");
                }
            }
            else
            {
                escenario.ESCENARIO_NUM = 1;
            }


            // Verifica el número de escenario y realiza acciones específicas
            switch (escenario.ESCENARIO_NUM)
            {
                case 1:

                    // crear transacción

                    // Escenario 1: No está registrado en DGME

                    var outputParamsCalidad = General.EjecutarProcedimientoAlmacenado(
                                                    _connectionStrings.SQL_18_PRUEBAS,
                                                    "SP_SET_EG_CALIDAD",
                                                    new
                                                    {
                                                        idPuesto = request.IdPuesto,
                                                        idCalidad = 0, // Se envía como 0 porque es la primera vez
                                                        nacionalidad = request.Nacionalidad,
                                                        profesion = request.Profesion,
                                                        tipoDocumento = request.TipoDocumento,
                                                        primerApellido = request.PrimerApellido,
                                                        segundoApellido = request.SegundoApellido ?? string.Empty,
                                                        nombre = request.Nombre,
                                                        conocidoComo = request.ConocidoComo ?? string.Empty,
                                                        sexo = request.Sexo,
                                                        fechaNacimiento = request.FechaNacimiento,
                                                        lugarNacimiento = request.LugarNacimiento,
                                                        estadoCivil = request.EstadoCivil,
                                                        nivelAcademico = request.NivelAcademico,
                                                        nombrePadre = request.NombrePadre,
                                                        nombreMadre = request.NombreMadre,
                                                        tieneIncapacidad = request.TieneIncapacidad == 1,
                                                        patriaPotestad = request.PatriaPotestad ?? string.Empty,
                                                        representanteLegal = request.RepresentanteLegal ?? string.Empty,
                                                        fechaSalidaPaisOrigen = request.FechaSalidaPaisOrigen,
                                                        fechaIngresoCR = request.FechaIngresoCR,
                                                        numeroDocumento = request.NumeroDocumento,
                                                        telefonoHabitacion = request.TelefonoHabitacion ?? string.Empty,
                                                        telefonoCelular = request.TelefonoCelular ?? string.Empty,
                                                        telefonoTrabajo = request.TelefonoTrabajo ?? string.Empty,
                                                        fax = request.Fax ?? string.Empty,
                                                        correo = request.Correo,
                                                        funcionario = funcionario,
                                                        consentimiento = request.Consentimiento,
                                                        cambioGenero = request.CambioGenero,
                                                        primerApellidoPadre = request.PrimerApellidoPadre ?? string.Empty,
                                                        segundoApellidoPadre = request.SegundoApellidoPadre ?? string.Empty,
                                                        primerApellidoMadre = request.PrimerApellidoMadre ?? string.Empty,
                                                        segundoApellidoMadre = request.SegundoApellidoMadre ?? string.Empty,
                                                        donaOrganos = request.DonaOrganos,
                                                        fechaDonacion = request.FechaDonacion,
                                                        IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString()
                                                    },
                                                    outputParams =>
                                                    {
                                                        // Definir los parámetros de salida
                                                        outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                                        outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                                                    }
                                                );

                    // Obtener los valores de salida
                    int resultCalidad = outputParamsCalidad.Get<int>("RESULT");
                    string idCalidad = outputParamsCalidad.Get<string>("MENSAJE");


                    General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, "SP_SET_EG_DIRECCION", HttpContext, new
                    {
                        idPuesto = request.IdPuesto, // preguntar si se genera una inconsistencia si tomamos el 135
                        idCalidad = idCalidad,
                        provincia = request.Provincia,
                        canton = request.Canton,
                        distrito = request.Distrito,
                        direccion = request.Direccion?.ToUpper(),
                        tipoDireccion = request.TipoDireccion,
                        funcionario = funcionario,
                        IPUsuario = IPUsuario,
                    });

                    General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, "SP_SET_EG_HISTORIAL_EXTRANJERO", HttpContext, new
                    {
                        idPuesto = request.IdPuesto,
                        idCalidad = idCalidad,
                        funcionario = funcionario,
                        IPUsuario = IPUsuario,
                    });


                    var outputParamsExpediente = General.EjecutarProcedimientoAlmacenado(
                                                                                            _connectionStrings.SQL_18_PRUEBAS,
                                                                                            "SP_SET_EX_EXPEDIENTE",
                                                                                            new
                                                                                            {
                                                                                                numeroExpediente = 0, // Se establece en cero porque es la primera vez
                                                                                                idPuesto = request.IdPuesto,
                                                                                                idPuestoCalidad = request.IdPuesto, //Nota Franklin: No es este idCalidad, debe ser este request.IdPuesto
                                                                                                idCalidad = idCalidad, // ??? (Ajustar según la lógica de negocio)
                                                                                                apoderado = request.RepresentanteLegal ?? string.Empty, // Validar porque viene en el request
                                                                                                donaOrganos = request.DonaOrganos,
                                                                                                tramiteYa = 0, // Se envía un número cero porque no se sabe si existe el trámite
                                                                                                numExpRefugio = 0, // Preguntar de dónde sacamos ese trámite
                                                                                                funcionario = funcionario,
                                                                                                IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString(),
                                                                                                estado_expediente = 1 // Al ser un expediente nuevo
                                                                                            },
                                                                                            outputParams =>
                                                                                            {
                                                                                                // Definir los parámetros de salida
                                                                                                outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                                                                                outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                                                                                            }
                                                                                        );

                    // Obtener los valores de salida
                    int resultExpediente = outputParamsExpediente.Get<int>("RESULT");
                    string numeroExpediente = outputParamsExpediente.Get<string>("MENSAJE");

                    var outputParams = General.EjecutarProcedimientoAlmacenado(
                                                                                 _connectionStrings.SQL_18_PRUEBAS,
                                                                                 "SP_SET_EX_SOLICITUD",
                                                                                 new
                                                                                 {
                                                                                     numeroExpediente = numeroExpediente,
                                                                                     idPuesto = request.IdPuesto,
                                                                                     idPuestoCalidad = request.IdPuesto, // al crear uno nuevo se crea uno
                                                                                     idCalidad = idCalidad,
                                                                                     tipoSolicitud = request.TipoSolicitud,
                                                                                     condicionMigratoria = request.CondicionMigratoria,
                                                                                     condicionLaboral = request.CondicionLaboral,
                                                                                     numeroSolicitud = 0,
                                                                                     PueIdExpediente = request.IdPuesto,
                                                                                     funcionario = funcionario,
                                                                                     IPUsuario = IPUsuario,
                                                                                     estado_expediente = 1
                                                                                 },
                                                                                 outputParams =>
                                                                                 {
                                                                                     // Definir los parámetros de salida
                                                                                     outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                                                                     outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                                                                                 }
                                                                             );



                    // Obtener los valores de salida
                    int result2 = outputParams.Get<int>("RESULT");
                    string numSolicitud2 = outputParams.Get<string>("MENSAJE");


                    foreach (var item in request.plantillaRequisitos)
                    {

                        General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, "SP_SET_EX_SOLICITUD_REQUISITO", HttpContext, new
                        {
                            numeroExpediente = numeroExpediente,
                            idPuesto = request.IdPuesto,
                            idCalidad = idCalidad,
                            tipoSolicitud = request.TipoSolicitud,
                            condicionMigratoria = request.CondicionMigratoria,
                            condicionLaboral = request.CondicionLaboral,
                            numeroSolicitud = numSolicitud2,          // resultado del SP anterior
                            idDocumento = item.DocId,
                            planReqNaturaleza = request.PlanReqNaturaleza.ToUpper(),
                            RequisitoPresente = item.Registrado.Trim().ToUpper(),
                            RequisitoURL = item.UrlDoc?.Trim() ?? null,
                            funcionario = funcionario,
                            IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString(),
                            Requisito_estado = 1        // es un valor nuevo
                        });
                    }
                    break;

                case 2:
                    // Escenario 2: Tiene calidad pero no expediente en Sinex

                    outputParamsExpediente = General.EjecutarProcedimientoAlmacenado(
                                                                                            _connectionStrings.SQL_18_PRUEBAS,
                                                                                            "SP_SET_EX_EXPEDIENTE",
                                                                                            new
                                                                                            {
                                                                                                numeroExpediente = 0, // Se establece en cero porque es la primera vez
                                                                                                idPuesto = request.IdPuesto,
                                                                                                idCalidad = DGMEResponse.Calidad,
                                                                                                idPuestoCalidad = DGMEResponse.PuestoCalidad, //Nota Franklin: No es este DGMEResponse.Calidad, debe ser este DGMEResponse.PuestoCalidad
                                                                                                apoderado = request.RepresentanteLegal ?? string.Empty,
                                                                                                donaOrganos = request.DonaOrganos,
                                                                                                tramiteYa = 0, // Se envía un número cero porque no se sabe si existe el trámite
                                                                                                numExpRefugio = 0, // Preguntar de dónde sacamos ese trámite
                                                                                                funcionario = funcionario,
                                                                                                IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString(),
                                                                                                estado_expediente = 1 // Al ser un expediente nuevo
                                                                                            },
                                                                                            outputParams =>
                                                                                            {
                                                                                                // Definir los parámetros de salida
                                                                                                outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                                                                                outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                                                                                            }
                                                                                        );

                    // Obtener los valores de salida
                    resultExpediente = outputParamsExpediente.Get<int>("RESULT");
                    numeroExpediente = outputParamsExpediente.Get<string>("MENSAJE");


                    General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, "SP_SET_EG_HISTORIAL_EXTRANJERO", HttpContext, new
                    {
                        idPuesto = request.IdPuesto, // Valor por defecto
                        idCalidad = DGMEResponse.Calidad,
                        funcionario = funcionario,
                        IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    });


                    outputParams = General.EjecutarProcedimientoAlmacenado(
                                                                            _connectionStrings.SQL_18_PRUEBAS,
                                                                            "SP_SET_EX_SOLICITUD",
                                                                            new
                                                                            {
                                                                                numeroExpediente = numeroExpediente,
                                                                                idPuesto = request.IdPuesto,
                                                                                idPuestoCalidad = DGMEResponse.PuestoCalidad, //Nota Franklin: No es este DGMEResponse.Calidad, debe ser este DGMEResponse.PuestoCalidad
                                                                                idCalidad = DGMEResponse.Calidad,
                                                                                tipoSolicitud = request.TipoSolicitud,
                                                                                condicionMigratoria = request.CondicionMigratoria,
                                                                                condicionLaboral = request.CondicionLaboral,
                                                                                numeroSolicitud = 0,
                                                                                PueIdExpediente = request.IdPuesto,
                                                                                funcionario = funcionario,
                                                                                IPUsuario = IPUsuario,
                                                                                estado_expediente = 1
                                                                            },
                                                                            outputParams =>
                                                                            {
                                                                                // Definir los parámetros de salida
                                                                                outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                                                                outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                                                                            }
                                                                        );

                    // Obtener los valores de salida
                    result2 = outputParams.Get<int>("RESULT");
                    numSolicitud2 = outputParams.Get<string>("MENSAJE");


                    foreach (var item in request.plantillaRequisitos)
                    {

                        General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, "SP_SET_EX_SOLICITUD_REQUISITO", HttpContext, new
                        {
                            numeroExpediente = DGMEResponse.NumeroExpediente,
                            idPuesto = request.IdPuesto,
                            idCalidad = DGMEResponse.Calidad,
                            tipoSolicitud = request.TipoSolicitud,
                            condicionMigratoria = request.CondicionMigratoria,
                            condicionLaboral = request.CondicionLaboral,
                            numeroSolicitud = numSolicitud2,          // resultado del SP anterior
                            idDocumento = item.DocId,
                            planReqNaturaleza = request.PlanReqNaturaleza,
                            RequisitoPresente = item.Registrado.Trim().ToUpper(),
                            RequisitoURL = item.UrlDoc?.Trim() ?? null,
                            funcionario = funcionario,
                            IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString(),
                            Requisito_estado = 1        // es un valor nuevo
                        });
                    }
                    break;

                case 3:
                    // Escenario 3: Ya tiene expediente en Sinex

                    var outputParams3 = General.EjecutarProcedimientoAlmacenado(
                                        _connectionStrings.SQL_18_PRUEBAS,
                                        "SP_SET_EX_SOLICITUD",
                                        new
                                        {
                                            numeroExpediente = DGMEResponse.NumeroExpediente,
                                            idPuesto = request.IdPuesto,
                                            idPuestoCalidad = request.IdPuesto,
                                            idCalidad = DGMEResponse.Calidad,
                                            tipoSolicitud = request.TipoSolicitud,
                                            condicionMigratoria = request.CondicionMigratoria,
                                            condicionLaboral = request.CondicionLaboral,
                                            numeroSolicitud = 0,
                                            PueIdExpediente = request.IdPuesto,
                                            funcionario = funcionario,
                                            IPUsuario = IPUsuario,
                                            estado_expediente = 1
                                        },
                                        outputParams =>
                                        {
                                            // Definir los parámetros de salida
                                            outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                            outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                                        }
                                    );

                    // Obtener los valores de salida
                    int result = outputParams3.Get<int>("RESULT");
                    string numSolicitud = outputParams3.Get<string>("MENSAJE");


                    foreach (var item in request.plantillaRequisitos)
                    {

                        General.ConsultaSQLServer<dynamic>(_connectionStrings.SQL_18_PRUEBAS, "SP_SET_EX_SOLICITUD_REQUISITO", HttpContext, new
                        {
                            numeroExpediente = DGMEResponse.NumeroExpediente,
                            idPuesto = request.IdPuesto,
                            idCalidad = DGMEResponse.Calidad,
                            tipoSolicitud = request.TipoSolicitud,
                            condicionMigratoria = request.CondicionMigratoria,
                            condicionLaboral = request.CondicionLaboral,
                            numeroSolicitud = numSolicitud,          // resultado del SP anterior
                            idDocumento = item.DocId,
                            planReqNaturaleza = request.PlanReqNaturaleza,
                            RequisitoPresente = item.Registrado.Trim().ToUpper(),
                            RequisitoURL = item.UrlDoc?.Trim() ?? null,
                            funcionario = funcionario,
                            IPUsuario = HttpContext.Connection.RemoteIpAddress?.ToString(),
                            Requisito_estado = 1        // es un valor nuevo
                        });
                    }
                    break;
                case 0:
                    Ok("Proceso completado para todos los escenario ");
                    break;

                default:
                    return BadRequest("Escenario no válido.");
            }

            return Ok("Proceso completado para el escenario " + escenario.ESCENARIO_NUM);
        }


        [HttpPost("CrearResolucion")]
        public async Task<IActionResult> CrearResolucion([FromForm] CrearResolucionRequest request)
        {
            string querySQL = @"INSERT INTO EX_RESOLUCION(PUE_ID, TIP_IDENTIFICADOR, RES_NUMERO, TIP_RESOLUCION, PUE_ID_ID, SOL_NUM_SOLICITUD, RES_PRONUNCIACION, RES_FECHA_EMISION, FIR_IDENTIFICADOR, RES_MAYORIA_EDAD, RES_ESTADO, RES_ARCHIVO, RES_USR_INSERT, RES_DATE_INSERT) 
                        VALUES (@GI_PUESTO_MIGRA, 2, @LL_CONSECUTIVO_RESOLUCION, 'I', @GI_PUESTO_SOL, @GL_SOLICITUD, @ID_PRONUNCIAMIENTO, @LDTM_FECHA_EMISION, @LI_FIRMA, @LI_MAYORIA_EDAD, 1, @LS_RESOLUCION, @LS_USUARIO, @LDTM_FECHA_EMISION)";

            var parameters = new
            {
                GI_PUESTO_MIGRA = request.IdPuestoCalidad, //cual id debo poner aquí
                LL_CONSECUTIVO_RESOLUCION = request.Pronunciamiento, 
                GI_PUESTO_SOL = request.IdPuestoSolicitud,
                GL_SOLICITUD = request.NumeroSolicitud,
                ID_PRONUNCIAMIENTO = request.Pronunciamiento,
                LDTM_FECHA_EMISION = DateTime.Now,
                LI_FIRMA = request.IdFirma,
                LI_MAYORIA_EDAD = request.MayoriaEdad,
                LS_RESOLUCION = request.ResolucionPdf != null ? Convert.ToBase64String(await GetFileBytesAsync(request.ResolucionPdf)) : null,
                LS_USUARIO = request.Funcionario
            };

            int result = General.InsertarSQLConParametros(_connectionStrings.extranjeria, querySQL, parameters, HttpContext);

            if (result >0) { 
            
            
            }

            // Respuesta exitosa
            var response = new CrearResolucionResponse
            {
                PuestoResolucion = request.IdPuestoCalidad,
                Identificador = request.Identificador,
                NumeroResolucion = request.Pronunciamiento
            };

            return Ok(response);
        }

        private async Task<byte[]> GetFileBytesAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }


        #region Lógica para crear apoderado

        [HttpPost("CrearApoderado")]
        public async Task<IActionResult> CrearApoderado([FromBody] CrearApoderadoRequest request)
        {
            try
            {
                
                // Respuesta exitosa
                var response = new CrearApoderadoResponse
                {
                    IdConsecutivoApo = "0"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Manejo de errores internos
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Descripcion = "Error interno del servidor"
                });
            }
        }

        #endregion

        #region crear estudio técnico

        [HttpPost("CrearEstudioTecnico")]
        public async Task<IActionResult> CrearEstudioTecnico([FromBody] CrearEstudioTecnicoRequest request)
        {
            try
            {

                //var outputParams3 = General.EjecutarProcedimientoAlmacenado(
                //                        _connectionStrings.SQL_18_PRUEBAS,
                //                        "SP_SET_EX_SOLICITUD",
                //                        new
                //                        {
                //                            numeroExpediente = DGMEResponse.NumeroExpediente,
                //                            idPuesto = request.IdPuesto,
                //                            idPuestoCalidad = request.IdPuesto,
                //                            idCalidad = DGMEResponse.Calidad,
                //                            tipoSolicitud = request.TipoSolicitud,
                //                            condicionMigratoria = request.CondicionMigratoria,
                //                            condicionLaboral = request.CondicionLaboral,
                //                            numeroSolicitud = 0,
                //                            PueIdExpediente = request.IdPuesto,
                //                            funcionario = funcionario,
                //                            IPUsuario = IPUsuario,
                //                            estado_expediente = 1
                //                        },
                //                        outputParams =>
                //                        {
                //                            // Definir los parámetros de salida
                //                            outputParams.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                //                            outputParams.Add("MENSAJE", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);
                //                        }
                //                    );

                // Obtener los valores de salida
                //int result = outputParams3.Get<int>("RESULT");
                //string numSolicitud = outputParams3.Get<string>("MENSAJE");

                // Respuesta exitosa
                var response = new CrearEstudioTecnicoResponse
                {
                    PuestoEstudio = request.IdPuestoEstudio,
                    NumeroEstudio = 0
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Manejo de errores internos
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Descripcion = "Error interno del servidor"
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("TesterQuerySybase")]
        public async Task<IActionResult> getTestSybase(string querySQL)
        {
            var condicionesMigratorias = General.ConsultaSQL<dynamic>(_connectionStrings.extranjeria, querySQL, HttpContext);

            return Ok(condicionesMigratorias);
        }


        #endregion

        #region Crear Notificación

        [HttpPost("CrearNotificacion")]
        public async Task<IActionResult> CrearNotificacion([FromForm] CrearNotificacionRequest request)
        {
            try
            {
                // Respuesta exitosa
                var response = new CrearNotificacionResponse
                {
                    PuestoNotificacion = request.IdPuestoNotificacion,
                    NumeroNotificacion = 0
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Manejo de errores internos
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Descripcion = "Error interno del servidor"
                });
            }
        }

        #endregion

        #region Crear documento único.

        [HttpPost("CrearDocumentoUnico")]
        public async Task<IActionResult> CrearDocumentoUnico([FromBody] CrearDocumentoUnicoRequest request)
        {
            try
            {
              
                // Respuesta exitosa
                var response = new CrearDocumentoUnicoResponse
                {
                    DocumentoUnico = "0",
                    FechaRenovacion = request.FechaRenovacion
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Manejo de errores internos
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Descripcion = "Error al emitir el número de documento único"
                });
            }
        }
        #endregion



        #region Actualizar documento único

        [HttpPost("ActualizarDocumentoUnico")]
        public async Task<IActionResult> ActualizarDocumentoUnico([FromBody] ActualizarDocumentoUnicoRequest request)
        {
            try
            {

                // Respuesta exitosa
                var response = new ActualizarDocumentoUnicoResponse
                {
                    DocumentoUnico = "0",
                    FechaRenovacion = request.FechaRenovacion
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Manejo de errores internos
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Descripcion = "Error al actualizar el documento único"
                });
            }
        }

        #endregion
    }
}
