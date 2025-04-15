using System.Diagnostics;
using System.Text;
using System.Threading.Tasks; //   for httpcontext
using Microsoft.AspNetCore.Http;     //for httpcontext

    public class RequestLoggingMiddlewares
    {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    //holds reference to the next middleware in the pipeline
        public RequestLoggingMiddlewares(RequestDelegate next, ILoggerFactory logger)
        {
        _next = next;
        _logger = logger.CreateLogger("CustoMiddleware");

    }
    public async Task InvokeAsync(HttpContext context)
        {
        //log request details
        //Debug.WriteLine($"Request Method:{context.Request.Method}");
        //Debug.WriteLine($"Request Method:{context.Request.Path}");
        //call the next middleware in the pipeline
        // await _next(context);
        //log response details
        // Debug.WriteLine($"Response Status Code:{context.Response.StatusCode}");

        var logFilePath = "Logs/request_log.txt";
        var logDetails = $"method:{context.Request.Method},path:{context.Request.Path},Time:{DateTime.UtcNow}\n";



        //Ensure the directory exist
        Directory.CreateDirectory("Logs");

        await File.AppendAllTextAsync(logFilePath, logDetails);

        await _next(context);

        }

    
}


