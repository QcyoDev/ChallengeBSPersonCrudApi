using System.Text.Json.Serialization;

namespace PersonCrud.Api.Dtos
{
    public class StatisticsDto
    {
        [JsonPropertyName("cantidad_hombres")]
        public int TotalMales { get; set; }

        [JsonPropertyName("cantidad_mujeres")]
        public int TotalFemales { get; set; }

        [JsonPropertyName("porcentaje_argentinos")]
        public decimal ArgentinesPercent { get; set; }
    }
}
