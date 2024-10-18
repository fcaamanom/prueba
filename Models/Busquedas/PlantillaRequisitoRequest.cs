using WSTramitaYa.Models;

namespace ApiExtranjeros.Models.Busquedas
{
    public class PlantillaRequisitoRequest
    {
        public int IdTipoSolicitud { get; set; }
        public int IdCondicionMigratoria { get; set; }
        public int IdCondicionLaboral { get; set; }
        public string NaturalezaTramite { get; set; }
    }
}
