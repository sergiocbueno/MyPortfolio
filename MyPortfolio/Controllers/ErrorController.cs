using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyPortfolio.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error/Handler/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            _logger.LogInformation($"[HttpStatusCodeHandler] An error occurred! Error code: [{statusCode.ToString()}]");
            ViewBag.ErrorCode = statusCode;

            switch(statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Page not found!";
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry an error occurred! We are trying to solve it as soon as possible, please try again later!";
                    break;
            }

            return View("Error");
        }
    }
}