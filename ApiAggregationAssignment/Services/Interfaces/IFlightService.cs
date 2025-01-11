using ApiAggregationAssignment.DTOs;

namespace ApiAggregationAssignment.Services.Interfaces
{
    public interface IFlightService
    {
        public Task<FlightsResponseDTO?> GetFlightsAsync(double latitude, double longitude, string? originCountry);
    }
}
