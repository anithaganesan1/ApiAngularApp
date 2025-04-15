using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace ApiAngularApp.Exception
{
    public class GlobalExceptionhandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionhandler> _logger;

        public GlobalExceptionhandler(ILogger<GlobalExceptionhandler>logger)
        {
            _logger = logger;

        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            if (exception is TimeoutException)
            {
                await MyException.ExceptionMessage(httpContext, exception, HttpStatusCode.RequestTimeout, "A timeout occurred");
                return true;
            }
            if (exception is ArgumentException)
            {
                await MyException.ExceptionMessage(httpContext, exception, HttpStatusCode.BadRequest, "A Bad Request occurred");
                return true;
            }
            else 
            {
                await MyException.ExceptionMessage(httpContext, exception, HttpStatusCode.InternalServerError, "An Exception Error occurred");

                return true;
            }
            return false;
        }
    }
}
