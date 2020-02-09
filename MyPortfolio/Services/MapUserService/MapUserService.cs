using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MyPortfolio.Services.MapUserService
{
    public class MapUserService : IMapUserService
    {
        private readonly IBaseRepository<AccessMap> _accessMapRepository;

        public MapUserService (IBaseRepository<AccessMap> accessMapRepository)
        {
            _accessMapRepository = accessMapRepository;
        }

        public void GetUserLocationByIpAddress(IPAddress ipAddress, string geoLocationDBPath)
        {
            CityResponse city = null;

            using (var reader = new DatabaseReader(geoLocationDBPath))
            {
                city = reader.City(ipAddress);
            }

            if (!string.IsNullOrWhiteSpace(city?.City?.Name))
            {
                var cityInDb = _accessMapRepository.GetByExpressionMultiThread(x => x.City.Equals(city.City.Name.ToLower())).SingleOrDefault();

                if (cityInDb == null)
                {
                    var newCity = new AccessMap { City = city.City.Name.ToLower() };
                    _accessMapRepository.SaveMultiThreadIncludingSaveContext(newCity);
                }
            }
        }

        public IList<AccessMapViewModel> FindUserInsideMap (IPAddress ipAddress, string geoLocationDBPath)
        {
            const string itIsYou = "Hey, you found yourself, it's you here!";
            const string itIsNotYou = "Unlucky, this was another person access, try again!";
            CityResponse city = null;

            using (var reader = new DatabaseReader(geoLocationDBPath))
            {
                city = reader.City(ipAddress);
            }

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

            return accessMaps;
        }
    }
}
