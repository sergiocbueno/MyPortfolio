using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Hosting;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
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
            var test2 =_accessMapRepository.GetById(1);
            using (var reader = new DatabaseReader(geoLocationDBPath))
            {
                var city = reader.City("92.251.65.190");//(ipAddress);
            }
        }

        //public void GetUserInsideMap ()
    }
}
