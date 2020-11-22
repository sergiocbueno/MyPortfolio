using Microsoft.AspNetCore.Builder;

namespace MyPortfolio.Middleware
{
    public static class ReverseProxyHttpsEnforcerExtensions
    {
        public static IApplicationBuilder UseReverseProxyHttpsEnforcer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReverseProxyHttpsEnforcer>();
        }
    }
}