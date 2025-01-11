using ApiAggregationAssignment.DTOs;
using ApiAggregationAssignment.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregationAssignment.Tests
{
    public class FlightServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly FlightService _flightService;

        public FlightServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _flightService = new FlightService(_httpClient);
        }

        [Fact]
        public async Task GetFlightsAsync_ReturnsFlightsData_WhenResponseIsValid()
        {
            // Arrange
            var expectedResponse = new FlightsResponseDTO
            {
                States = new List<List<object?>?> // Ensure that this matches your DTO's expected structure
                {
                    new List<object?> { "Flight123", "Origin", "Destination", 12345, 40.7128, -74.0060, 1000, 500 }
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _flightService.GetFlightsAsync(40.7128, -74.0060, null);

            // Assert
            Assert.NotNull(result); 
            Assert.NotNull(result.States); 
            Assert.Single(result.States); 

            
            var firstState = result.States[0];
            Assert.NotNull(firstState); 

            Assert.NotNull(firstState); 
            Assert.NotNull(firstState[0]); 


            Assert.Equal(expectedResponse.States[0]?[0], firstState[0]); 
        }

        [Fact]
        public async Task GetFlightsAsync_ReturnsNull_WhenResponseIsEmpty()
        {
            // Arrange
            var jsonResponse = JsonConvert.SerializeObject(new FlightsResponseDTO { States = null });
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>()) 
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _flightService.GetFlightsAsync(40.7128, -74.0060, null);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.States); 
        }

    }
}

