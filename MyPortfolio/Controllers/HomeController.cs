using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPortfolio.Models;
using MyPortfolio.Services.MapUserService;

namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapUserService _mapUserService;
        private readonly IConfiguration _configuration;
        private const string _endOfGeoLocationDBPath = "\\GeoLocationDB\\GeoLite2-City.mmdb";

        public HomeController(IHostingEnvironment hostingEnvironment, 
            IMapUserService mapUserService,
            IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapUserService = mapUserService;
            _configuration = configuration;
        }

        public IActionResult About()
        {
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Ajax calls

        [HttpPost]
        public JsonResult PersistNewAccess(string ipAddress)
        {
            var geoLocationDBPath = _hostingEnvironment.ContentRootPath + _endOfGeoLocationDBPath;
            _ = Task.Run(() => _mapUserService.GetUserLocationByIpAddress(ipAddress, geoLocationDBPath));

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult GetMapChartData(string ipAddress)
        {
            var geoLocationDBPath = _hostingEnvironment.ContentRootPath + _endOfGeoLocationDBPath;
            var accessMapViewModel = _mapUserService.FindUserInsideMap(ipAddress, geoLocationDBPath);

            return Json(new { success = true, data = accessMapViewModel, apiKey = _configuration.GetConnectionString("GoogleApiKey") });
        }

        #endregion
    }
}
