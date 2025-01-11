using ApiAggregationAssignment.DTOs;
using ApiAggregationAssignment.Filters;
using ApiAggregationAssignment.Services.Interfaces;
using Newtonsoft.Json;

namespace ApiAggregationAssignment.Services
{
    public class FlightService : IFlightService
    {
        private readonly HttpClient _httpClient;

        public FlightService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<FlightsResponseDTO?> GetFlightsAsync(double latitude, double longitude, string? originCountry)
        {
            try
            {
                latitude = Convert.ToInt32(latitude);
                longitude = Convert.ToInt32(longitude);

                var url = $"https://opensky-network.org/api/states/all?lamin={latitude - 1}&lamax={latitude + 1}&lomin={longitude - 1}&lomax={longitude + 1}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<FlightsResponseDTO>(content);

                if (result?.States != null && !string.IsNullOrEmpty(originCountry))
                {
                    var filteredStates = FlightFilters.FlightOriginFilter(result, originCountry);
                    result.States = filteredStates;
                }
                
               return result;

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error in FlightService: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in FlightService: {ex.Message}");
                throw;
            }
        }
    }
}
