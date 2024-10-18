using ApiExtranjeros.Models;
using ApiExtranjeros.Models.Seguridad;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ApiExtranjeros.Common
{
    public class ApiSeguridad
    {
        private readonly HttpClient _httpClient;
        public readonly AppSettings _appSettings;

        public ApiSeguridad(HttpClient httpClient, IOptions<Configuration> configuration)
        {
            _httpClient = httpClient;
            _appSettings = configuration.Value.appSettings;
        }

        /// <summary>
        /// Obtener el token desde el sistema de seguridad
        /// </summary>
        /// <returns>El token del sistema de seguridad</returns>
        public async Task<string> GetJWTAsync()
        {
            var requestBody = new
            {
                UserName = _appSettings.userSeguridad,
                Password = _appSettings.passSeguridad,
                System = _appSettings.userSeguridad
            };

            var jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_appSettings.urlSeguridad}api/login/authenticate", content);

            response.EnsureSuccessStatusCode(); // lanza exception si la solicitud no fué exitosa.

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetJWTAsync(LoginRequest login)
        {
            var jsonRequest = System.Text.Json.JsonSerializer.Serialize(login);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_appSettings.urlSeguridad}api/login/authenticateV2", content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Acceso no autorizado. Verifique sus credenciales.");
            }

            response.EnsureSuccessStatusCode(); // lanza exception si la solicitud no fué exitosa.

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Funcion que consume el WebApi Seguridad para verificar si el usuario y contraseña son validos.
        /// </summary>
        /// <param name="usuario">El objeto con usuario y contraseña</param>
        /// <returns>Un mensaje de valido o falso.</returns>
        public async Task<HttpResponseMessage> VerificarContrasenaAsync(MOD_USUARIO_ACTD usuario, LoginRequest login)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string body = System.Text.Json.JsonSerializer.Serialize(usuario);

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var token = await GetJWTAsync(login);

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("No se pudo generar token de autenticación en el api de seguridad");
            }
            token= token.Trim('\"');

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            response = await _httpClient.PostAsync($"{_appSettings.urlSeguridad}api/Seguridad/validarUsuarioServicio", content);
            response.EnsureSuccessStatusCode(); // lanza exception si la solicitud no fué exitosa.

            return response;
        }

        public async Task<HttpResponseMessage> ObtenerPermisosAsync(MOD_SISTEMA sistema)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string body = JsonConvert.SerializeObject(sistema);

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var token = await GetJWTAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("No se pudo generar token de autenticación en el api de seguridad");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            response = await _httpClient.PostAsync($"{_appSettings.urlSeguridad}api/Seguridad/obtenerpermisos", content);
            response.EnsureSuccessStatusCode(); // lanza exception si la solicitud no fué exitosa.

            return response;
        }

        /// <summary>
        /// Llama a la funcion que verifica si el usuario tiene los permisos en el WebApi de seguridad
        /// </summary>
        /// <param name="permiso">Los permisos a validar</param>
        /// <returns>El objeto permiso con los valores. </returns>
        public async Task<PermisoSistema> TienePermisoAsync(PermisoSistema permiso)
        {
            string body = JsonConvert.SerializeObject(permiso);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var token = await GetJWTAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("No se pudo generar token de autenticación en el api de seguridad");
            }
            token = token.Trim('\"');

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{_appSettings.urlSeguridad}api/Seguridad/tienepermiso", content);

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();

            var permisoSistema = JsonConvert.DeserializeObject<PermisoSistema>(responseContent);

            if (permisoSistema is null) throw new Exception("No se pudieron obtener los permisos del sistema");

            permisoSistema.Exito = true;
            permisoSistema.Mensaje = "";

            return permisoSistema;
        }

        /// <summary>
        /// Verifica si un usuario tiene el permiso en seguridad por el nombre del permiso
        /// </summary>
        /// <param name="usuario">El login del usuario a verificar</param>
        /// <param name="permiso">El nombre del permiso</param>
        /// <returns>Si tiene el permiso o no</returns>

        public async Task<bool> TienePermisoUsuarioAsync(string usuario, string permiso, string sistema)
        {
            PermisoSistema per = new PermisoSistema
            {
                Sistema = sistema,
                Usuario = usuario,
                Permisos = new ObjetoPermiso[]
                {
                    new ObjetoPermiso { ID_Permiso = -1, Nombre = permiso, Bandera = false }
                }
            };

            PermisoSistema resultado = await TienePermisoAsync(per);

            return resultado.Exito && resultado.Permisos[0].Bandera;
        }
    }
}
