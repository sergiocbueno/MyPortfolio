using System.Net;

namespace MyPortfolio.Services.MapUserService
{
    public interface IMapUserService
    {
        void GetUserLocationByIpAddress(IPAddress ipAddress, string geoLocationDBPath);
    }
}
