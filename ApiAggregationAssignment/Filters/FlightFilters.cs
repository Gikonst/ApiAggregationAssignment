using ApiAggregationAssignment.DTOs;
using Newtonsoft.Json.Linq;

namespace ApiAggregationAssignment.Filters
{
    public static class FlightFilters
    {
        public static List<List<object?>?> FlightOriginFilter(FlightsResponseDTO flightsResponse, string originCountry)
        {
            if(flightsResponse == null || string.IsNullOrEmpty(originCountry))
                return new List<List<object?>?>();

            var states = flightsResponse.States;
            if (states == null)
                return new List<List<object?>?>();

            var filteredStates = states
                .Where(state => state != null &&
                                state.Count() > 2 &&
                                state[2]?.ToString()?.ToLower() == originCountry.ToLower())
                .ToList();

            return filteredStates;
        }
    }
}
