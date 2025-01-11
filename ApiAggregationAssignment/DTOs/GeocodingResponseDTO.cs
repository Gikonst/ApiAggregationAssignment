using ApiAggregationAssignment.Models.Geocoding;
using Newtonsoft.Json;

namespace ApiAggregationAssignment.DTOs
{
    public class GeocodingResponseDTO
    {
        [JsonProperty("results")]
        public IEnumerable<GeocodingResult?>? ResultObjects { get; set; }
    }
}
