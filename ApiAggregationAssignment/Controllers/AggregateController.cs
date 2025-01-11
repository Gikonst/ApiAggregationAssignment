using ApiAggregationAssignment.DTOs;
using ApiAggregationAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiAggregationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        private readonly IGeocodingService _geocodingService;
        private readonly IWeatherService _weatherService;
        private readonly IFlightService _flightService;

        public AggregateController
            (
                IGeocodingService geocodingService,
                IWeatherService weatherService,
                IFlightService flightService
            )
        {
            _geocodingService = geocodingService;
            _weatherService = weatherService;
            _flightService = flightService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAggregatedData
        (
            [FromQuery] string location,
            [FromQuery] string? originCountry 
        )

        {
            try
            {

                var coordinates = await _geocodingService.GetCoordinatesAsync(location);

                if(coordinates == null)
                {
                    return BadRequest("Could not get coordinates for the specified location.");
                }
                var weatherData = await _weatherService.GetWeatherAsync(coordinates.Lat, coordinates.Lng);
                if (weatherData == null)
                {
                    return NotFound("Weather data not found.");
                }


                var flightData = await _flightService.GetFlightsAsync(coordinates.Lat, coordinates.Lng, originCountry);
                
                if (flightData == null || flightData.States == null)
                {
                    return NotFound("No flights found.");
                }

                var aggregatedResult = new AggregatedResultResponseDTO
                {
                    Location = location,
                    Weather = weatherData,
                    Flights = flightData
                };

                return Ok(aggregatedResult);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Issue communicating with the third party APIs : {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error occured: {ex.Message}");
            }
        }
    }


}
