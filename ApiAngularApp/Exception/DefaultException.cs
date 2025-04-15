using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiAngularApp.Exception
{
    public class DefaultException : IExceptionHandler
    {
        private readonly ILogger<DefaultException> _logger;

        public DefaultException(ILogger<DefaultException>logger) 
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            System.Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unexception");
            
            //throw new NotImplementedException();

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status =(int)HttpStatusCode.InternalServerError,
                Type=exception.GetType().Name,
                Title="An  unexpected error occured",
                Detail= exception.Message,
                Instance=$"{httpContext.Request.Method} {httpContext.Request.Path}"
            });
            return true;
        }
    }
}
