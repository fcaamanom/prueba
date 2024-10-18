using Newtonsoft.Json;

namespace ApiExtranjeros.Models
{
    public class AppSettings
    {
        public string JWT_SECRET_KEY { get; set; }
        public string JWT_AUDIENCE_TOKEN { get; set; }
        public string JWT_ISSUER_TOKEN { get; set; }
        public string JWT_EXPIRE_MINUTES { get; set; }
        public string urlSeguridad { get; set; }
        public string userSeguridad { get; set; }
        public string passSeguridad { get; set; }
        public string usuarios { get; set; }
        public string ipsPermitidas { get; set; }
        public string prefijoSistema { get; set; }
    }

    public class ConnectionStrings
    {
        public string general { get; set; }
        public string extranjeria { get; set; }
        public string financiero { get; set; }
        public string seguridad { get; set; }
        public string SQL_18_PRUEBAS { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }

        [JsonProperty("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; }

        [JsonProperty("Microsoft.Hosting.Lifetime")]
        public string MicrosoftHostingLifetime { get; set; }

        [JsonProperty("Microsoft.EntityFrameworkCore.Database.Command")]
        public string MicrosoftEntityFrameworkCoreDatabaseCommand { get; set; }
    }

    public class Configuration
    {
        public Logging Logging { get; set; }
        public AppSettings appSettings { get; set; }
        public ConnectionStrings connectionStrings { get; set; }
        public string AllowedHosts { get; set; }
    }

}
