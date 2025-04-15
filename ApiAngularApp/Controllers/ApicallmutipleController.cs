using ApiAngularApp.Models.Domain;
using ApiAngularApp.Respositories.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAngularApp.Exception;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System;
namespace ApiAngularApp.Controllers
{
    public class ApicallmutipleController : Controller
    {
        private readonly ApiService _apiService;

        public ApicallmutipleController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("consume-multiple")]
        public async Task<IActionResult> ConsumeMultipleApis()
        {
            var api1Url = "https://jsonplaceholder.typicode.com/posts";
            var api2Url = "https://jsonplaceholder.typicode.com/users";

            // Start both API calls asynchronously
            var api1Task = _apiService.GetAsync<List<Post>>(api1Url);
            var api2Task = _apiService.GetAsync<List<User>>(api2Url);

            // Wait for both tasks to complete
            await Task.WhenAll(api1Task, api2Task);

            var api1Result = await api1Task; // Posts data
            var api2Result = await api2Task; // Users data

            return Ok(new
            {
                Api1Data = api1Result,
                Api2Data = api2Result
            });
        }
        [HttpGet("consume-multiple-safe")]
        public async Task<IActionResult> ConsumeMultipleApisSafely()
        {
            var api1Url = "https://jsonplaceholder.typicode.com/posts";
            var api2Url = "https://jsonplaceholder.typicode.com/users";

            try
            {
                var api1Task = _apiService.GetAsync<List<Post>>(api1Url);
                var api2Task = _apiService.GetAsync<List<User>>(api2Url);

                await Task.WhenAll(api1Task, api2Task);

                var api1Result = await api1Task;
                var api2Result = await api2Task;

                return Ok(new
                {
                    Api1Data = api1Result,
                    Api2Data = api2Result
                });
            }
            catch (exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while consuming APIs", Details = ex.Message });
            }
        }

    }

    [Serializable]
    internal class exception : System.Exception
    {
        public exception()
        {
        }

        public exception(string? message) : base(message)
        {
        }

        public exception(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
