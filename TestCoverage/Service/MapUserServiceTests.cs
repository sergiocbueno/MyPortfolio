using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Services.MapUserService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
        public void SaveUserLocationByIpAddressAsync_WhenIpAddressIsNull_ShouldNotSaveAndNoDatabaseCalls()
        {
            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(null).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenIpAddressIsEmpty_ShouldNotSaveAndNoDatabaseCalls()
        {
            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(string.Empty).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenCityNotFound_ShouldNotSaveAndNoDatabaseCalls()
        {
            // Setup
            var ipAddress = "127.0.0.1";

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"ip_address\":\"127.0.0.1\",\"city\":null,\"city_geoname_id\":null,\"region\":null,\"region_iso_code\":null,\"region_geoname_id\":null,\"postal_code\":null,\"country\":null,\"country_code\":null,\"country_geoname_id\":null,\"country_is_eu\":null,\"continent\":null,\"continent_code\":null,\"continent_geoname_id\":null,\"longitude\":null,\"latitude\":null,\"security\":{\"is_vpn\":false}}"),
                })
                .Verifiable();

            _httpClientFactoryMock.Setup(x => x.CreateClient(nameof(MapUserService))).Returns(new HttpClient(handlerMock.Object));

            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(ipAddress).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(nameof(MapUserService)), Times.Once);
            handlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"{BASE_ABSTRACT_GEOLOCATION_API}?api_key={_appsetttingsMock["AbstractGeolocationApiKey"]}&ip_address={ipAddress}"),
                ItExpr.IsAny<CancellationToken>());
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_ResponseDifferentThanOk_ShouldNotSaveAndNoDatabaseCalls()
        {
            // Setup
            var ipAddress = "185.2.144.123";

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest })
                .Verifiable();

            _httpClientFactoryMock.Setup(x => x.CreateClient(nameof(MapUserService))).Returns(new HttpClient(handlerMock.Object));

            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(ipAddress).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(nameof(MapUserService)), Times.Once);
            handlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"{BASE_ABSTRACT_GEOLOCATION_API}?api_key={_appsetttingsMock["AbstractGeolocationApiKey"]}&ip_address={ipAddress}"),
                ItExpr.IsAny<CancellationToken>());
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenCityAlreadyExistsInDb_ShouldNotSaveDataInDatabase()
        {
            // Setup test environment
            var ipAddress = "113.197.7.177";

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"ip_address\":\"113.197.7.177\",\"city\":\"Melbourne\",\"city_geoname_id\":2158177,\"region\":\"Victoria\",\"region_iso_code\":\"VIC\",\"region_geoname_id\":2145234,\"postal_code\":\"3004\",\"country\":\"Australia\",\"country_code\":\"AU\",\"country_geoname_id\":2077456,\"country_is_eu\":false,\"continent\":\"Oceania\",\"continent_code\":\"OC\",\"continent_geoname_id\":6255151,\"longitude\":144.9799,\"latitude\":-37.8411,\"security\":{\"is_vpn\":false},\"timezone\":{\"name\":\"Australia/Melbourne\",\"abbreviation\":\"AEDT\",\"gmt_offset\":11,\"current_time\":\"06:46:09\",\"is_dst\":true},\"flag\":{\"emoji\":\"🇦🇺\",\"unicode\":\"U+1F1E6 U+1F1FA\",\"png\":\"https://static.abstractapi.com/country-flags/AU_flag.png\",\"svg\":\"https://static.abstractapi.com/country-flags/AU_flag.svg\"},\"currency\":{\"currency_name\":\"Australian Dollars\",\"currency_code\":\"AUD\"},\"connection\":{\"autonomous_system_number\":7575,\"autonomous_system_organization\":\"Australian Academic and Research Network AARNet\",\"connection_type\":\"Corporate\",\"isp_name\":\"Australian Academic and Research Network\",\"organization_name\":\"Australian Academic and Research Network\"}}"),
                })
                .Verifiable();

            _httpClientFactoryMock.Setup(x => x.CreateClient(nameof(MapUserService))).Returns(new HttpClient(handlerMock.Object));

            var sydneyCity = new AccessMap { Id = 5, City = "melbourne" };
            _accessMapRepositoryMock.Setup(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()))
                .Returns(new List<AccessMap> { sydneyCity });

            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(ipAddress).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(nameof(MapUserService)), Times.Once);
            handlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"{BASE_ABSTRACT_GEOLOCATION_API}?api_key={_appsetttingsMock["AbstractGeolocationApiKey"]}&ip_address={ipAddress}"),
                ItExpr.IsAny<CancellationToken>());
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Once);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void SaveUserLocationByIpAddressAsync_WhenCityDoesNotExistsInDb_ShouldSaveCityInDatabase()
        {
            // Setup test environment
            var ipAddress = "113.197.7.177";

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"ip_address\":\"113.197.7.177\",\"city\":\"Melbourne\",\"city_geoname_id\":2158177,\"region\":\"Victoria\",\"region_iso_code\":\"VIC\",\"region_geoname_id\":2145234,\"postal_code\":\"3004\",\"country\":\"Australia\",\"country_code\":\"AU\",\"country_geoname_id\":2077456,\"country_is_eu\":false,\"continent\":\"Oceania\",\"continent_code\":\"OC\",\"continent_geoname_id\":6255151,\"longitude\":144.9799,\"latitude\":-37.8411,\"security\":{\"is_vpn\":false},\"timezone\":{\"name\":\"Australia/Melbourne\",\"abbreviation\":\"AEDT\",\"gmt_offset\":11,\"current_time\":\"06:46:09\",\"is_dst\":true},\"flag\":{\"emoji\":\"🇦🇺\",\"unicode\":\"U+1F1E6 U+1F1FA\",\"png\":\"https://static.abstractapi.com/country-flags/AU_flag.png\",\"svg\":\"https://static.abstractapi.com/country-flags/AU_flag.svg\"},\"currency\":{\"currency_name\":\"Australian Dollars\",\"currency_code\":\"AUD\"},\"connection\":{\"autonomous_system_number\":7575,\"autonomous_system_organization\":\"Australian Academic and Research Network AARNet\",\"connection_type\":\"Corporate\",\"isp_name\":\"Australian Academic and Research Network\",\"organization_name\":\"Australian Academic and Research Network\"}}"),
                })
                .Verifiable();

            _httpClientFactoryMock.Setup(x => x.CreateClient(nameof(MapUserService))).Returns(new HttpClient(handlerMock.Object));

            _accessMapRepositoryMock.Setup(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()))
                .Returns(new List<AccessMap>());

            // Action
            _mapUserService.SaveUserLocationByIpAddressAsync(ipAddress).Wait();

            // Asserts
            _httpClientFactoryMock.Verify(x => x.CreateClient(nameof(MapUserService)), Times.Once);
            handlerMock.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"{BASE_ABSTRACT_GEOLOCATION_API}?api_key={_appsetttingsMock["AbstractGeolocationApiKey"]}&ip_address={ipAddress}"),
                ItExpr.IsAny<CancellationToken>());
            _accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Once);
            _accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Once);
        }

        #endregion
    }
}
