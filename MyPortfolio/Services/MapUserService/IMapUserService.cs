using MyPortfolio.Models;
using System.Collections.Generic;
using System.Net;

namespace MyPortfolio.Services.MapUserService
{
    public interface IMapUserService
    {
        void GetUserLocationByIpAddress(IPAddress ipAddress, string geoLocationDBPath);
        IList<AccessMapViewModel> FindUserInsideMap(IPAddress ipAddress, string geoLocationDBPath);
    }
}
