using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.Logging;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPortfolio.Services.MapUserService
{
    public class MapUserService : IMapUserService
    {
        private readonly IBaseRepository<AccessMap> _accessMapRepository;
        private readonly ILogger _logger;

        public MapUserService (IBaseRepository<AccessMap> accessMapRepository, ILogger<MapUserService> logger)
        {
            _accessMapRepository = accessMapRepository;
            _logger = logger;
        }

        public void GetUserLocationByIpAddress(string ipAddress, string geoLocationDBPath)
        {
            _logger.LogInformation($"[GetUserLocationByIpAddress] received ipAddress [{ipAddress}] and geoLocationDBPath [{geoLocationDBPath}]");
            var city = GetCityByIpAddress(ipAddress, geoLocationDBPath);

            if (!string.IsNullOrWhiteSpace(city?.City?.Name))
            {
                var cityInDb = _accessMapRepository.GetByExpressionMultiThread(x => x.City.Equals(city.City.Name.ToLower())).SingleOrDefault();

                if (cityInDb == null)
                {
                    var newCity = new AccessMap { City = city.City.Name.ToLower() };
                    _logger.LogInformation($"[GetUserLocationByIpAddress] will save new city [{newCity.City}] in database");
                    _accessMapRepository.SaveMultiThreadIncludingSaveContext(newCity);
                }
            }
        }

        public IList<AccessMapViewModel> FindUserInsideMap (string ipAddress, string geoLocationDBPath)
        {
            _logger.LogInformation($"[FindUserInsideMap] received ipAddress [{ipAddress}] and geoLocationDBPath [{geoLocationDBPath}]");
            const string itIsYou = "Hey, you found yourself, it's you here!";
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            var city = GetCityByIpAddress(ipAddress, geoLocationDBPath);
            var allAccesses = _accessMapRepository.GetAllSingleThread();
            var accessMaps = new List<AccessMapViewModel>();

            foreach(var access in allAccesses)
            {
                var accessMapTemp = new AccessMapViewModel
                {
                    CityName = access.City,
                    Message = city?.City?.Name != null && access.City.Equals(city.City.Name.ToLower()) ? itIsYou : itIsNotYou
                };

                accessMaps.Add(accessMapTemp);
            }

            _logger.LogInformation($"[FindUserInsideMap] returning list with [{accessMaps.Count()}] elements");
            return accessMaps;
        }

        #region Private Methods

        private CityResponse GetCityByIpAddress (string ipAddress, string geoLocationDBPath)
        {
            _logger.LogInformation($"[GetCityByIpAddress] received ipAddress [{ipAddress}] and geoLocationDBPath [{geoLocationDBPath}]");
            CityResponse city = null;
            DatabaseReader reader = null;

            try
            {
                reader = new DatabaseReader(geoLocationDBPath);
                city = reader.City(ipAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR!] GetCityByIpAddress method throw this exception [{ex.Message}]");
                city = null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            _logger.LogInformation($"[GetCityByIpAddress] returning city as [{city?.City?.Name}]");
            return city;
        }

        #endregion
    }
}
