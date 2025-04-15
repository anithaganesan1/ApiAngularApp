using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ApiAngularApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutipleApicallController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MutipleApicallController (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        [HttpGet("fetch-multiple")]
        public async Task<IActionResult> FetchMultipleAsync()
        {
            var urls = new List<string>
            {
                "https://jsonplaceholder.typicode.com/posts",
                "https://jsonplaceholder.typicode.com/comments",
                "https://jsonplaceholder.typicode.com/todos"
            };

           //var results = await FetchUrlsAsync(urls);
            var results = await FetchUrlsWithLimitAsync(urls,10);


            return Ok(results);
        }
        
        private async Task<List<string>> FetchUrlsWithLimitAsync(IEnumerable<string> urls, int maxConcurrency)
        {

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);

            using var semaphore = new SemaphoreSlim(maxConcurrency);
            var tasks = urls.Select(async url =>
            {
                await semaphore.WaitAsync();
                try
                {
                    //return await FetchUrlsAsync(urls);
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();

                }
                finally
                {
                    semaphore.Release();
                }
            });
            return (await Task.WhenAll(tasks)).ToList();
        }


    }
}
