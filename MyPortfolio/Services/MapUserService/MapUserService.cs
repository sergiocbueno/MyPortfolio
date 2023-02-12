using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyPortfolio.Services.MapUserService
{
    public class MapUserService : IMapUserService
    {
        private const string CLIENT_FACTORY_NAME = nameof(MapUserService);
        private const string BASE_ABSTRACT_GEOLOCATION_API = "https://ipgeolocation.abstractapi.com/v1/";

        private readonly IBaseRepository<AccessMap> _accessMapRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MapUserService (
            IBaseRepository<AccessMap> accessMapRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<MapUserService> logger)
        {
            _accessMapRepository = accessMapRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SaveUserLocationByIpAddressAsync(string ipAddress)
        {
            _logger.LogInformation($"[SaveUserLocationByIpAddressAsync] received ipAddress [{ipAddress}]");

            var cityName = await GetCityNameByIPAddressAsync(ipAddress);

            if (string.IsNullOrEmpty(cityName))
                return;

            var cityInDb = _accessMapRepository.GetByExpressionMultiThread(map => map.City.Equals(cityName.ToLower())).SingleOrDefault();

            if (cityInDb != null)
                return;

            var newCity = new AccessMap { City = cityName.ToLower() };
            _logger.LogInformation($"[SaveUserLocationByIpAddressAsync] will save new city [{newCity.City}] in database");
            _accessMapRepository.SaveMultiThreadIncludingSaveContext(newCity);
        }

        public async Task<IList<AccessMapViewModel>> FindUserInsideMapAsync(string ipAddress)
        {
            _logger.LogInformation($"[FindUserInsideMap] received ipAddress [{ipAddress}]");
            const string itIsYou = "Hey, you found yourself, it's you here!";
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            var cityName = await GetCityNameByIPAddressAsync(ipAddress);
            var allAccesses = _accessMapRepository.GetAllSingleThread();
            var accessMaps = new List<AccessMapViewModel>();

            foreach(var access in allAccesses)
            {
                var accessMapTemp = new AccessMapViewModel
                {
                    CityName = access.City,
                    Message = !string.IsNullOrEmpty(cityName) && access.City.Equals(cityName.ToLower()) ? itIsYou : itIsNotYou
                };

                accessMaps.Add(accessMapTemp);
            }

            _logger.LogInformation($"[FindUserInsideMap] returning list with [{accessMaps.Count()}] elements");
            return accessMaps;
        }

        #region Private Methods

        private async Task<string> GetCityNameByIPAddressAsync(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return null;

            var httpClient = _httpClientFactory.CreateClient(CLIENT_FACTORY_NAME);
            
            var apiKey = _configuration.GetValue<string>("AbstractGeolocationApiKey");
            var url = $"{BASE_ABSTRACT_GEOLOCATION_API}?api_key={apiKey}&ip_address={ipAddress}";
            var httpResponse = await httpClient.GetAsync(url);

			if (httpResponse.StatusCode != HttpStatusCode.OK)
                return null;

            var textResponse = await httpResponse.Content.ReadAsStringAsync();
            dynamic jsonResponse = JObject.Parse(textResponse);
            string cityName = jsonResponse.city;

            return cityName;
        }

        #endregion
    }
}
