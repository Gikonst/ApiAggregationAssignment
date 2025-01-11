using ApiAggregationAssignment.DTOs;
using ApiAggregationAssignment.Models.Weather;
using ApiAggregationAssignment.Services.Interfaces;
using Newtonsoft.Json;

namespace ApiAggregationAssignment.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResponseDTO?> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var formattedLatitude = latitude.ToString("F1"); 
                var formattedLongitude = longitude.ToString("F1");
                var apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={formattedLatitude}&longitude={formattedLongitude}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum";
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var resultList = JsonConvert.DeserializeObject<List<WeatherResponseDTO>>(content);

           
                return resultList?.FirstOrDefault();
            }
            catch (HttpRequestException ex) 
            {
                Console.WriteLine($"Http error in WeatherService: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in WeatherService: {ex.Message}");
                throw;
            }
        }
    }
}
