using ApiAggregationAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregationAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeocodingController : ControllerBase
    {
        private readonly IGeocodingService _geocodingService;

        public GeocodingController(IGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;
        }

        [HttpGet("GetGeocodingData")]
        public async Task<IActionResult> GetGeocodingData([FromQuery] string location)
        {
            try
            {
                var coordinates = await _geocodingService.GetCoordinatesAsync(location);

                return Ok(coordinates);
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
