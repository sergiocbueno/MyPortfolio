using MyPortfolio.Models;
using System.Collections.Generic;

namespace MyPortfolio.Services.MapUserService
{
    public interface IMapUserService
    {
        void GetUserLocationByIpAddress(string ipAddress, string geoLocationDBPath);
        IList<AccessMapViewModel> FindUserInsideMap(string ipAddress, string geoLocationDBPath);
    }
}
