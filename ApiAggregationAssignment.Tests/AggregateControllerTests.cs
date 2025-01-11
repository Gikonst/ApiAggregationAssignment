using ApiAggregationAssignment.Controllers;
using ApiAggregationAssignment.DTOs;
using ApiAggregationAssignment.Models.Geocoding;
using ApiAggregationAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregationAssignment.Tests
{
    public class AggregateControllerTests
    {
        [Fact]
        public async Task GetAggregatedData_ReturnsAggregatedResult_WhenServicesReturnValidData()
        {
            // Arrange
            var mockGeocodingService = new Mock<IGeocodingService>();
            var mockWeatherService = new Mock<IWeatherService>();
            var mockFlightService = new Mock<IFlightService>();

            mockGeocodingService.Setup(s => s.GetCoordinatesAsync(It.IsAny<string>()))
                .ReturnsAsync(new GeocodingGeometry { Lat = 40.7128, Lng = -74.0060 } );

            mockWeatherService.Setup(s => s.GetWeatherAsync(40.7128, -74.0060))
                .ReturnsAsync(new WeatherResponseDTO { Latitude = 40.7128, Longitude = -74.0060 });

            mockFlightService.Setup(s => s.GetFlightsAsync(40.7128, -74.0060, null)) 
                .ReturnsAsync(new FlightsResponseDTO { States = new List<List<object?>?>() });

            var controller = new AggregateController(
                mockGeocodingService.Object,
                mockWeatherService.Object,
                mockFlightService.Object
            );

            // Act
            var result = await controller.GetAggregatedData("New York", null); 

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var aggregatedResult = Assert.IsType<AggregatedResultResponseDTO>(okResult.Value);
            Assert.Equal("New York", aggregatedResult.Location);
            Assert.NotNull(aggregatedResult.Weather);
            Assert.NotNull(aggregatedResult.Flights);
        }
    }
}
