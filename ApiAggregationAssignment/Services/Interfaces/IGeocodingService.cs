using ApiAggregationAssignment.Models.Geocoding;

namespace ApiAggregationAssignment.Services.Interfaces
{
    public interface IGeocodingService
    {
        public Task<GeocodingGeometry?> GetCoordinatesAsync(string location);
    }
}
