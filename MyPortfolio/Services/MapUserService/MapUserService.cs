using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyPortfolio.Services.MapUserService
{
    public class MapUserService : IMapUserService
    {
        const string CLIENT_FACTORY_NAME = nameof(MapUserService);
        const string BASE_ABSTRACT_GEOLOCATION_API = "https://ipgeolocation.abstractapi.com/v1/";

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

            var httpClient = _httpClientFactory.CreateClient(CLIENT_FACTORY_NAME);
            
            var apiKey = _configuration.GetValue<string>("AbstractGeolocationApiKey");
            var url = $"{BASE_ABSTRACT_GEOLOCATION_API}?api_key={apiKey}&ip_address={ipAddress}";
            var httpResponse = await httpClient.GetAsync(url);

			if (httpResponse.StatusCode == HttpStatusCode.OK)
			{
                var text = await httpResponse.Content.ReadAsStringAsync();
			}

            // var cityName = GetCityByIpAddress(ipAddress, geoLocationDBPath);

            // if (!string.IsNullOrWhiteSpace(cityName))
            // {
            //     var cityInDb = _accessMapRepository.GetByExpressionMultiThread(x => x.City.Equals(city.City.Name.ToLower())).SingleOrDefault();

            //     if (cityInDb == null)
            //     {
            //         var newCity = new AccessMap { City = city.City.Name.ToLower() };
            //         _logger.LogInformation($"[GetUserLocationByIpAddress] will save new city [{newCity.City}] in database");
            //         _accessMapRepository.SaveMultiThreadIncludingSaveContext(newCity);
            //     }
            // }
        }

        public async Task<IList<AccessMapViewModel>> FindUserInsideMapAsync(string ipAddress)
        {
            _logger.LogInformation($"[FindUserInsideMap] received ipAddress [{ipAddress}]");
            const string itIsYou = "Hey, you found yourself, it's you here!";
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            var city = string.Empty;//GetCityByIpAddress(ipAddress, geoLocationDBPath);
            var allAccesses = _accessMapRepository.GetAllSingleThread();
            var accessMaps = new List<AccessMapViewModel>();

            foreach(var access in allAccesses)
            {
                var accessMapTemp = new AccessMapViewModel
                {
                    CityName = access.City,
                    Message = city != null && access.City.Equals(city.ToLower()) ? itIsYou : itIsNotYou
                    //Message = city?.City?.Name != null && access.City.Equals(city.City.Name.ToLower()) ? itIsYou : itIsNotYou
                };

                accessMaps.Add(accessMapTemp);
            }

            _logger.LogInformation($"[FindUserInsideMap] returning list with [{accessMaps.Count()}] elements");
            return accessMaps;
        }

        #region Private Methods

        #endregion
    }
}
