using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Services.MapUserService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;

namespace TestCoverage.Service
{
    [TestClass]
    public class MapUserServiceTests
    {
        private const string BASE_ABSTRACT_GEOLOCATION_API = "https://ipgeolocation.abstractapi.com/v1/";
        private readonly IDictionary<string, string> _appsetttingsMock = new Dictionary<string, string> {
            {"AbstractGeolocationApiKey", "UnitTestApiKey"}
        };

        private readonly Mock<IBaseRepository<AccessMap>> _accessMapRepositoryMock;
        private readonly Mock<ILogger<MapUserService>> _mapUserServiceLoggingMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly IConfiguration _configuration;
        private readonly MapUserService _mapUserService;

        public MapUserServiceTests()
		{
            _accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            _mapUserServiceLoggingMock = new Mock<ILogger<MapUserService>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(_appsetttingsMock).Build();
            _mapUserService = new MapUserService(_accessMapRepositoryMock.Object, _httpClientFactoryMock.Object, _configuration, _mapUserServiceLoggingMock.Object);
		}

        #region Method: SaveUserLocationByIpAddressAsync 

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenIpAddressIsNull_ShouldNotSaveAnyCallInDatabase()
        {
            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(null).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenIpAddressIsEmpty_ShouldNotSaveAnyCallInDatabase()
        {
            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(string.Empty).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenIpAddressIsInvalid_ShouldNotSaveAnyCallInDatabase()
        {
            // Setup
            var ipAddress = "127.0.0.1";

            var httpClientMock = new Mock<HttpClient>();
            _httpClientFactoryMock.Setup(x => x.CreateClient(nameof(MapUserService))).Returns(httpClientMock.Object);

            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(ipAddress).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(nameof(MapUserService)), Times.Once);
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        #endregion
    }
}
