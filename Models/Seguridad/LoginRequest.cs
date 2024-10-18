using Newtonsoft.Json;

namespace ApiExtranjeros.Models.Seguridad
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string? System { get; set; } = null;

    }
}
