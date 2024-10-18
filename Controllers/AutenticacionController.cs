
using ApiExtranjeros.Common;
using ApiExtranjeros.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using static ApiExtranjeros.Common.SybaseDBUtil;

namespace ApiExtranjeros.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class AutenticacionController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly ApiSeguridad _apiSeguridad;
        private readonly TokenGenerator _tokenGenerator;

        public AutenticacionController(ApiSeguridad apiSeguridad, IOptions<Configuration> configuration, TokenGenerator tokenGenerator)
        {
            _appSettings = configuration.Value.appSettings;
            _apiSeguridad = apiSeguridad;
            _tokenGenerator = tokenGenerator;

        }
        /// <summary>
        /// echoping 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("echoping")]
        public IActionResult EchoPing()
        {
            return Ok(true);

        }

        /// <summary>
        /// echouser 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("echouser")]
        public IActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        /// <summary>
        /// Verifica si el usuario esta permitido en el Config
        /// </summary>
        /// <param name="usuario">El usuario a validar</param>
        /// <returns>Si esta permitido true, sino false</returns>
        private bool verificarUsuarios(string usuario)
        {
            string cadena = _appSettings.usuarios;
            string[] users = cadena.Split(',');
            foreach (var us in users)
            {
                if (us == usuario) return true;
            }
            return false;
        }

        /// <summary>
        /// Login usuario
        /// </summary>
        /// <param name="login">El usuario a validar</param>
        /// <returns>Token</returns>
        /// 
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> loginAsync(Models.Seguridad.LoginRequest login)
        {
            if (login == null)
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);

            var modUsuario = General.GetUsuarioConfig(login, HttpContext, _appSettings.prefijoSistema);

            var respuesta = await _apiSeguridad.VerificarContrasenaAsync(modUsuario, login);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var token = _tokenGenerator.GenerateJwtToken(login.UserName);
                General.SybaseLog(HttpContext, tipoMensaje.Exito, "Token generado exitoso.");
                return Ok(token);
            }
            return Unauthorized();

        }


        /// <summary>
        /// Login usuario con clave Encriptada
        /// </summary>
        /// <param name="login">El usuario a validar</param>
        /// <returns>Token</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("loginEncriptedPass")]
        public async Task<IActionResult> LoginEncriptedPassAsync(Models.Seguridad.LoginRequest login)
        {
            if (login == null)
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);

            login.System =_appSettings.userSeguridad;

            login.Password = Utilidades.Base64Decode(login.Password);

            var modUsuario = General.GetUsuarioConfig(login, HttpContext, _appSettings.prefijoSistema, false);

            var respuesta = await _apiSeguridad.VerificarContrasenaAsync(modUsuario, login);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var token = _tokenGenerator.GenerateJwtToken(login.UserName);
                General.SybaseLog(HttpContext, tipoMensaje.Exito, "Token generado exitoso.");
                return Ok(token);
            }
            return Unauthorized();

        }
    }
}
