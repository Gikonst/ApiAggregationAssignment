using ApiAggregationAssignment.Models.Weather;
using Newtonsoft.Json;

namespace ApiAggregationAssignment.DTOs
{
    public class WeatherResponseDTO
    {
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("daily_units")]
        public WeatherDailyUnits? DailyUnits { get; set; }

        [JsonProperty("daily")]
        public WeatherDays? Daily {  get; set; }
    }
}
