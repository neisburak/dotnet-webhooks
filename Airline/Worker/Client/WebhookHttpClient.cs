using System.Net.Http.Headers;
using System.Text.Json;
using Worker.Models;

namespace Worker.Client;

public class WebhookHttpClient : IWebhookClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WebhookHttpClient> _logger;

    public WebhookHttpClient(IHttpClientFactory httpClientFactory, ILogger<WebhookHttpClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task SendAsync(string uri, ChangePayload payload, IDictionary<string, string> headers)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        foreach (var header in headers) request.Headers.Add(header.Key, header.Value);

        var content = JsonSerializer.Serialize(payload);
        request.Content = new StringContent(content);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        try
        {
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Unsuccessful: {ex.Message}");
        }
    }
}