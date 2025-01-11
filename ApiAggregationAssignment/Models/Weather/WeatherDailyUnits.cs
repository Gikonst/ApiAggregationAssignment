using Newtonsoft.Json;

namespace ApiAggregationAssignment.Models.Weather
{
    public class WeatherDailyUnits
    {
        [JsonProperty("time")]
        public string? Time {  get; set; }
        [JsonProperty("temperature_2m_max")]
        public string? MaxTemperature { get; set; }
        [JsonProperty("temperature_2m_min")]
        public string? MinTemperature { get; set; }

        [JsonProperty("precipitation_sum")]
        public string? Precipitation { get; set; }
    }
}
