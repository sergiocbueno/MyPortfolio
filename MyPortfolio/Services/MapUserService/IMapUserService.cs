using MyPortfolio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPortfolio.Services.MapUserService
{
    public interface IMapUserService
    {
        Task SaveUserLocationByIpAddressAsync(string ipAddress);
        Task<IList<AccessMapViewModel>> FindUserInsideMapAsync(string ipAddress);
    }
}
