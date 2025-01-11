namespace ApiAggregationAssignment.DTOs
{
    public class AggregatedResultResponseDTO
    {
        public string? Location { get; set; }
        public WeatherResponseDTO? Weather { get; set; } 
        public FlightsResponseDTO? Flights { get; set; }
    }
}
