using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CodeFlix.Catalog.EndToEndTests.Base;

public class ApiClient
{
    private readonly HttpClient _client;

    public ApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(string routePath, object payload)
    {
        var response = await _client.PostAsync(routePath, new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json));
        var responseAsString = await response.Content.ReadAsStringAsync() ?? "";
        if (string.IsNullOrWhiteSpace(responseAsString))
            return (response, default);

        var output = JsonSerializer.Deserialize<TOutput>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return (response, output);
    }
}
