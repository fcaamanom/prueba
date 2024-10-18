using Dapper;
using System.Net;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using AdoNetCore.AseClient;
using static ApiExtranjeros.Common.SybaseDBUtil;
using ApiExtranjeros.Models.Seguridad;
using System.Data.SqlClient;
using System.Security.Claims;

namespace ApiExtranjeros.Common
{
    public static class General
    {
        public static HttpResponseMessage CrearRespuestaJson<T>(HttpStatusCode codigoEstado, T contenido)
        {
            var respuesta = new HttpResponseMessage(codigoEstado)
            {
                Content = new StringContent(JsonConvert.SerializeObject(contenido), Encoding.UTF8, "application/json")
            };
            return respuesta;
        }

        public static IEnumerable<T> ConsultaSQL<T>(string stringConnection, string querySQL, HttpContext httpContext)
        {
            using IDbConnection db = new AseConnection(stringConnection);
            SybaseLog(httpContext, tipoMensaje.Exito, "Consulta exitosa");
            return db.Query<T>(querySQL).ToList();
        }

        public static IEnumerable<T> ConsultaSQLServer<T>(
               string stringConnection,
               string querySQL,
               HttpContext httpContext,
               object parametros = null) //
        {
            using IDbConnection db = new SqlConnection(stringConnection);

            // Ejecuta la consulta SQL con o sin parámetros
            return db.Query<T>(querySQL, parametros).ToList();
        }


        public static int InsertarSQLConParametros(string stringConnection, string querySQL, object parameters, HttpContext httpContext)
        {
            using IDbConnection db = new AseConnection(stringConnection);
            try
            {
                int filasAfectadas = db.Execute(querySQL, parameters);
                SybaseLog(httpContext, tipoMensaje.Exito, "Inserción exitosa");
                return filasAfectadas;
            }
            catch (Exception ex)
            {
                SybaseLog(httpContext, tipoMensaje.Error, $"Error en la inserción: {ex.Message}");
                throw;
            }
        }

        public static IEnumerable<T> ConsultaSQLConParametros<T>(string stringConnection, string querySQL, object parameters, HttpContext httpContext)
        {
            using IDbConnection db = new AseConnection(stringConnection);
            SybaseLog(httpContext, tipoMensaje.Exito, "Consulta exitosa");
            return db.Query<T>(querySQL, parameters).ToList();
        }

        public static async Task<bool> ValidarPermisoAsync(HttpContext httpContext, ApiSeguridad seguridad, string tipoPermiso)
        {
            var userName = httpContext.User.Claims.FirstOrDefault().Value;
            //var userName = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            bool tienePermiso = await seguridad.TienePermisoUsuarioAsync(userName, tipoPermiso, seguridad._appSettings.prefijoSistema);

            if (!tienePermiso)
            {
                SybaseLog(httpContext, tipoMensaje.Error, "No tiene permiso.");
            }

            return tienePermiso;
        }

        public static void SybaseLog(HttpContext httpContext, tipoMensaje tipoMensaje, string mensaje)
        {
            //SybaseDBUtil.escribirMsgLog(httpContext.Request.Path, httpContext.User.Identity.Name, httpContext.Connection.RemoteIpAddress.ToString(), tipoMensaje, mensaje, null);
        }

        public static MOD_USUARIO_ACTD GetUsuarioConfig(LoginRequest login, HttpContext httpContext, string prefijoSistema, bool isPassEncoded = false)
        {
            MOD_USUARIO_ACTD usuario = new MOD_USUARIO_ACTD();
            usuario.USUARIO = login.UserName;
            usuario.CLAVE = isPassEncoded ? Utilidades.Base64Decode(login.Password) : login.Password;
            usuario.Log_Usuario = login.UserName;
            usuario.Log_Sistema = prefijoSistema;
            usuario.pref_id = prefijoSistema;
            usuario.BIT_NOMBRE_RED = httpContext.Request.Host.Value;
            usuario.BIT_NOMBRE_PC = Dns.GetHostName();
            return usuario;
        }


        public static string ObtenerUsuarioId(HttpContext context)
        {
            if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                // Aquí debes implementar la lógica para obtener el ID del usuario autenticado
                // Por ejemplo, si estás usando Claims:
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                return userIdClaim?.Value ?? string.Empty;
            }

            return string.Empty; // Retorna null si el usuario no está autenticado
        }


        public static DynamicParameters EjecutarProcedimientoAlmacenado(
                                                                         string stringConnection,
                                                                         string storedProcedure,
                                                                         object parametros = null,
                                                                         Action<DynamicParameters> configureOutputParameters = null)
        {
            using IDbConnection db = new SqlConnection(stringConnection);

            // Crear los parámetros dinámicos
            var dynamicParameters = new DynamicParameters(parametros);

            // Configurar los parámetros de salida si se proporcionan
            configureOutputParameters?.Invoke(dynamicParameters);

            // Ejecutar el procedimiento almacenado
            db.Execute(storedProcedure, dynamicParameters, commandType: CommandType.StoredProcedure);

            // Retornar los parámetros dinámicos, incluyendo los valores de salida
            return dynamicParameters;
        }


    }
}
