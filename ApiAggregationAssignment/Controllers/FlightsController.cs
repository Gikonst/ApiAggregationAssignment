using ApiAggregationAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("GetFlightsData")]
        public async Task<IActionResult> GetFlightsData([FromQuery] double latitude, double longitude, string? originCountry)
        {
            try
            {
                var flightsData = await _flightService.GetFlightsAsync( latitude,  longitude, originCountry);

                return Ok(flightsData);
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
