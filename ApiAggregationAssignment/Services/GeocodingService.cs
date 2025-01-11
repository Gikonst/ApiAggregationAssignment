using ApiAggregationAssignment.DTOs;
using ApiAggregationAssignment.Models.Geocoding;
using ApiAggregationAssignment.Services.Interfaces;
using Newtonsoft.Json;

namespace ApiAggregationAssignment.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ICacheService _cacheService;


        public GeocodingService(HttpClient httpClient, IConfiguration configuration, ICacheService cacheService)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Geocoding:ApiKey"] ??
                throw new ArgumentNullException("Geocoding:ApiKey is not set in the configuration.");
            _cacheService = cacheService;
        }
        public async Task<GeocodingGeometry?> GetCoordinatesAsync(string location)
        {
            try
            {
                string cacheKey = $"Geocoding_{location}";

                return await _cacheService.GetOrCreateAsync(cacheKey,async () =>
               {
                   string encodedLocation = Uri.EscapeDataString(location);
                   var apiUrl = $"https://api.opencagedata.com/geocode/v1/json?q={encodedLocation}&key={_apiKey}";
                   var response = await _httpClient.GetAsync(apiUrl);
                   response.EnsureSuccessStatusCode();

                   var content = await response.Content.ReadAsStringAsync();
                   var result = JsonConvert.DeserializeObject<GeocodingResponseDTO>(content);

                   return result?.ResultObjects?.FirstOrDefault()?.Geometry;
               },
               TimeSpan.FromMinutes(10));
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Http error in GeocodingService: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in GeocodingService: {ex.Message}");
                throw;
            }
        }
    }
}
