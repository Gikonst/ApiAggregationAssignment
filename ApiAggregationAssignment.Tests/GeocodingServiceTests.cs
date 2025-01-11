using ApiAggregationAssignment.Services;
using ApiAggregationAssignment.Models.Geocoding;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using ApiAggregationAssignment.Services.Interfaces;

namespace ApiAggregationAssignment.Tests
{
    public class GeocodingServiceTests
    {
        [Fact]
        public async Task GetCoordinatesAsync_ReturnsCoordinates_WhenResponseIsValid()
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
                    Content = new StringContent(@"{
                        'results': [{
                            'geometry': { 'lat': 40.7128, 'lng': -74.0060 }
                        }]
                    }")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["Geocoding:ApiKey"]).Returns("notgivingout");

            var mockCacheService = new Mock<ICacheService>();
            mockCacheService
                .Setup(cache => cache.GetOrCreateAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<GeocodingGeometry>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<GeocodingGeometry?>>, TimeSpan>((key, factory, time) => factory());

            var service = new GeocodingService(httpClient, mockConfiguration.Object, mockCacheService.Object);

            // Act
            var result = await service.GetCoordinatesAsync("New York");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(40.7128, result.Lat);
            Assert.Equal(-74.0060, result.Lng);
        }

        [Fact]
        public async Task GetCoordinatesAsync_ThrowsException_WhenHttpClientFails()
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
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["Geocoding:ApiKey"]).Returns("notgivingout");

            var mockCacheService = new Mock<CacheService>(new MemoryCache(new MemoryCacheOptions()));


            var service = new GeocodingService(httpClient, mockConfiguration.Object, mockCacheService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetCoordinatesAsync("New York"));
        }
    }
}
