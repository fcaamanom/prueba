using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace ApiExtranjeros.Common
{
    internal class TokenValidationHandler : DelegatingHandler
    {
        private readonly Utilidades _utils;
        private readonly HttpContext  _httpContext;
        public TokenValidationHandler(HttpContext httpContext, Utilidades utilidades)
        {
            _utils = utilidades;
            _httpContext = httpContext;
;        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;
            string mensajeError;

            if (!accesoIP(request))
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
            }

            // determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                //SybaseDBUtil.escribirMsgLog("Autenticacion", "system", HttpContext.Current.Request.UserHostAddress, SybaseDBUtil.tipoMensaje.Error, "No autorizado", null);
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var secretKey = _utils.getConfig("appSettings:JWT_SECRET_KEY");
                var audienceToken = _utils.getConfig("appSettings:JWT_AUDIENCE_TOKEN");
                var issuerToken = _utils.getConfig("appSettings:JWT_ISSUER_TOKEN");
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

                SecurityToken securityToken;
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                // Extract and assign Current Principal and user
                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                //HttpContext.Current.User    = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                _httpContext.User  = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException st)
            {
                //SybaseDBUtil.escribirMsgLog("Validacion", "system", HttpContext.Current.Request.UserHostAddress, SybaseDBUtil.tipoMensaje.Error, "No autorizado", st);
                mensajeError = st.Message;
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                //SybaseDBUtil.escribirMsgLog("Validacion", "system", HttpContext.Current.Request.UserHostAddress, SybaseDBUtil.tipoMensaje.Error, "No autorizado", ex);
                mensajeError = ex.Message;
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        #region Seguridad bloqueo por IP

        public bool accesoIP(HttpRequestMessage request)
        {
             
            var ips = _utils.getConfig("appSettings:ipsPermitidas");
            //var ips = _utils.getConfig("appSettings:ipsPermitidas");
            if (!string.IsNullOrEmpty(ips))
            {
                var listado = ips.Split(',').ToList();
                //string ipAddressString = HttpContext.Current.Request.UserHostAddress;
                string ipAddressString = _httpContext.Request.Host.Value.ToString();
                var ip = listado
                    .Where(a => a.Trim()
                    .Equals(ipAddressString, StringComparison.InvariantCultureIgnoreCase))
                    .Any();
                return ip;
            }
            return true;
        }
        #endregion
    }
}
