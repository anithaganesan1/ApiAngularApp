using Microsoft.Data.Edm;

namespace ApiAngularApp
{

    public class CustMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger("CustoMiddleware");
        }
        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("Custom Middleware Initiate");
            await _next(httpContext);
        }

    }

    public static class CustMiddlewareExtensions
    {
        public static IApplicationBuilder Usecusmiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustMiddleware>();
                
                
                
         }

       
    }
}