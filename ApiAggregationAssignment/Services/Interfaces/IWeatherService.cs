using ApiAggregationAssignment.DTOs;

namespace ApiAggregationAssignment.Services.Interfaces
{
    public interface IWeatherService
    {
        public Task<WeatherResponseDTO?> GetWeatherAsync(double latitude, double longitude);
    }
}
