using ApiAggregationAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("GetWeatherData")]
        public async Task<IActionResult> GetWeatherData([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(latitude, longitude);
                return Ok(weatherData);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return StatusCode(500, $"Issue communicating with the third party APIs : {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error occured: {ex.Message}");
            }
        }
    }
}
