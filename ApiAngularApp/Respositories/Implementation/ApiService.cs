

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace ApiAngularApp.Respositories.Implementation
{
    public class ApiService
    {

        public readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient) {

            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

    }
}
