using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyPortfolio.Services.MapUserService;

namespace MyPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapUserService _mapUserService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public HomeController(IMapUserService mapUserService,
            IConfiguration configuration,
            ILogger<HomeController> logger)
        {
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
        public async Task<JsonResult> PersistNewAccess(string ipAddress)
        {
            _logger.LogInformation("[PersistNewAccess] AJAX call inside HomeController");
            await _mapUserService.SaveUserLocationByIpAddressAsync(ipAddress);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> GetMapChartData(string ipAddress)
        {
            _logger.LogInformation("[GetMapChartData] AJAX call inside HomeController");
            
            var accessMapViewModel = await _mapUserService.FindUserInsideMapAsync(ipAddress);

            return Json(new { success = true, data = accessMapViewModel, apiKey = _configuration.GetValue<string>("GoogleApiKey") });
        }

        #endregion
    }
}
