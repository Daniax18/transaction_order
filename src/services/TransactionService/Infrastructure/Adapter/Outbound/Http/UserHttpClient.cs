using System.Net.Http;
using System.Text.Json;
using TransactionService.Application.Dto;
using TransactionService.Application.Port.Outbound;

namespace TransactionService.Infrastructure.Adapter.Outbound.Http
{
    public class UserHttpClient : IUserService
    {

        private readonly HttpClient _httpClient;

        public UserHttpClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            Console.WriteLine($"BaseAddress: {_httpClient.BaseAddress}");
        }

        public async Task<Result<Dictionary<string, string>>> GetUserNameByIdsAsync(string[] ids)
        {
            if(ids.Length == 0) {
                return Result<Dictionary<string, string>>.NOk("No ids get");
            }
            try
            {
                var queryString = string.Join("&", ids.Select(id => $"ids={Uri.EscapeDataString(id)}"));
                var uri = $"names?{queryString}";
                Console.WriteLine(uri);

                var response = await _httpClient.GetAsync(uri);
                // var response = await _httpClient.GetAsync($"/names?{queryString}");    // TODO : Add the endpoint in a configuration file

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"HTTP error: {response.StatusCode}");
                    return Result<Dictionary<string, string>>.NOk($"HTTP error: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, string>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<Dictionary<string, string>>.Ok(result!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Exception message: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                return Result<Dictionary<string, string>>.NOk("Error on calling userHttp : " + ex.Message);
            }
        }
    }
}
