﻿using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyPortfolio.Services.MapUserService;
using Microsoft.Extensions.Hosting;

namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IMapUserService _mapUserService;
        private readonly IConfiguration _configuration;
        private readonly string _endOfGeoLocationDBPath = Path.DirectorySeparatorChar + "GeoLocationDB" + Path.DirectorySeparatorChar + "GeoLite2-City.mmdb";
        private readonly ILogger _logger;

        public HomeController(IHostEnvironment hostingEnvironment, 
            IMapUserService mapUserService,
            IConfiguration configuration,
            ILogger<HomeController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapUserService = mapUserService;
            _configuration = configuration;
            _logger = logger;
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

        #region Ajax calls

        [HttpPost]
        public JsonResult PersistNewAccess(string ipAddress)
        {
            _logger.LogInformation("[PersistNewAccess] AJAX call inside HomeController");
            var geoLocationDBPath = _hostingEnvironment.ContentRootPath + _endOfGeoLocationDBPath;
            _ = Task.Run(() => _mapUserService.GetUserLocationByIpAddress(ipAddress, geoLocationDBPath));

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult GetMapChartData(string ipAddress)
        {
            _logger.LogInformation("[GetMapChartData] AJAX call inside HomeController");
            var geoLocationDBPath = _hostingEnvironment.ContentRootPath + _endOfGeoLocationDBPath;
            var accessMapViewModel = _mapUserService.FindUserInsideMap(ipAddress, geoLocationDBPath);

            return Json(new { success = true, data = accessMapViewModel, apiKey = _configuration.GetConnectionString("GoogleApiKey") });
        }

        #endregion
    }
}
