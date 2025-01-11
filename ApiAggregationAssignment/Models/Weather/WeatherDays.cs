using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApiAggregationAssignment.Models.Weather
{
    public class WeatherDays
    {
        [JsonProperty("time")]
        public IEnumerable<DateTime?>? Time {  get; set; }
        [JsonProperty("temperature_2m_max")]
        public IEnumerable<double?>? MaxTemperature { get; set; }
        [JsonProperty("temperature_2m_min")]
        public IEnumerable<double?>? MinTemperature { get; set; }
        [JsonProperty("precipitation_sum")]
        public IEnumerable<double?>? PrecipitationSum { get; set; }

    }
}
