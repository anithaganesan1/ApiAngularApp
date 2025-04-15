using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiAngularApp.Exception
{
    public class MyException
    {
        public static async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            System.Exception exception, HttpStatusCode httpStatusCode,string Tittle)     
        {
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)httpStatusCode,
                Type = exception.GetType().Name,
                Title = Tittle,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            });
            return true;
        }

        internal static async Task ExceptionMessage(HttpContext httpContext, System.Exception exception, HttpStatusCode requestTimeout, string v)
        {
            throw new NotImplementedException();
        }
    }
}
