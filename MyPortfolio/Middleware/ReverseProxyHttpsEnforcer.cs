using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyPortfolio.Middleware
{
    public class ReverseProxyHttpsEnforcer
    {
        private const string ForwardedProtoHeader = "X-Forwarded-Proto";
        private readonly RequestDelegate _next;

        public ReverseProxyHttpsEnforcer(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;

            if (headers[ForwardedProtoHeader] == string.Empty || headers[ForwardedProtoHeader] == "https")
            {
                await _next(context);
            }
            else if (headers[ForwardedProtoHeader] != "https")
            {
                var withHttps = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                context.Response.Redirect(withHttps);
            }
        }
    }
}