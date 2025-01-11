using ApiAggregationAssignment.Services;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregationAssignment.Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task GetWeatherAsync_ReturnsWeatherData_WhenResponseIsValid()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"[
                {
                    'latitude': 40.7128,
                    'longitude': -74.0060,
                    'daily_units': { 'temperature_2m_max': 'C', 'temperature_2m_min': 'C' },
                    'daily': { 
                        'time': ['2023-01-01'], 
                        'temperature_2m_max': [5.0], 
                        'temperature_2m_min': [-1.0] 
                    }
                }
            ]")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new WeatherService(httpClient);

            // Act
            var result = await service.GetWeatherAsync(40.7128, -74.0060);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(40.7128, result.Latitude);
            Assert.Equal(-74.0060, result.Longitude);
            Assert.Equal(5.0, result?.Daily?.MaxTemperature?.First());
            Assert.Equal(-1.0, result?.Daily?.MinTemperature?.First());
        }

        [Fact]
        public async Task GetWeatherAsync_ThrowsException_WhenHttpClientFails()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("HTTP error"));

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new WeatherService(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync(40.7128, -74.0060));
        }
    }
}
