
using Newtonsoft.Json;

namespace ApiAggregationAssignment.DTOs
{
    public class FlightsResponseDTO
    {
        [JsonProperty("time")]
        public int? Time { get; set; }
        [JsonProperty("states")]
        public List<List<object?>?>? States { get; set; }

    }
}
