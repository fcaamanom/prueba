using System.Net;

namespace ApiExtranjeros.Common
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;

        public Utilidades()
        {
        }

        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Obtener configuraciones desde el appsettings.json
        /// </summary>
        /// <param name="value">La llave a buscar</param>
        /// <returns>El valor del config</returns>
        public string getConfig(string value)
        {
            try
            {
                string valor = _configuration.GetValue<string>(value).ToString();
                return valor;
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
