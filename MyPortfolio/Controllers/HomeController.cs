using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using MyPortfolio.Models;
using MyPortfolio.Services.MapUserService;

namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapUserService _mapUserService;
        private readonly IConfiguration _configuration;
        private const string _endOfGeoLocationDBPath = "\\GeoLocationDB\\GeoLite2-City.mmdb";

        public HomeController(IActionContextAccessor actionContextAccessor, 
            IHostingEnvironment hostingEnvironment, 
            IMapUserService mapUserService,
            IConfiguration configuration)
        {
            _actionContextAccessor = actionContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mapUserService = mapUserService;
            _configuration = configuration;
        }

        public IActionResult About()
        {
            var ipAddress = _actionContextAccessor.ActionContext.HttpContext.Connection.RemoteIpAddress;
            var geoLocationDBPath = _hostingEnvironment.ContentRootPath + _endOfGeoLocationDBPath;
            _ = Task.Run(() => _mapUserService.GetUserLocationByIpAddress(ipAddress, geoLocationDBPath));

            return View();
        }

        public IActionResult Education()
        {
            return View();
        }

        public IActionResult Experience()
        {
            return View();
        }

        public IActionResult Skills()
        {
            return View();
        }

        public IActionResult MappingAccess()
        {
            ViewData["GoogleApiKey"] = _configuration.GetConnectionString("GoogleApiKey");

            var ipAddress = _actionContextAccessor.ActionContext.HttpContext.Connection.RemoteIpAddress;
            var geoLocationDBPath = _hostingEnvironment.ContentRootPath + _endOfGeoLocationDBPath;
            var accessMapViewModel = _mapUserService.FindUserInsideMap(ipAddress, geoLocationDBPath);

            return View(accessMapViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
