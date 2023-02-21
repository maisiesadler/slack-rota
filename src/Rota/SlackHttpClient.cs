using System.Net.Http.Json;

namespace Rota;

public interface ISlackHttpClient
{
    Task Execute(string text);
}

public class SlackHttpClient : ISlackHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly SlackReporterConfiguration _config = new SlackReporterConfiguration();

    public SlackHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Execute(string text)
    {
        var channel = _config.Channel;

        var payload = new { channel, text };

        var url = "https://slack.com/api/chat.postMessage";
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.Token);

        var r = await _httpClient.PostAsJsonAsync(url, payload);

        System.Console.WriteLine(r.StatusCode);
        if (TryGetErrorMessage(await r.Content.ReadAsStringAsync(), out var errorMessage))
        {
            System.Console.WriteLine("Slack error: " + errorMessage);
        }
    }

    // Slack return a 200 status code even when the payload is an error - this is a work-around
    private bool TryGetErrorMessage(string content, out string? errorMessage)
    {
        if (content.Contains("error"))
        {
            errorMessage = content;
            return true;
        }
        errorMessage = "";
        return false;
    }
}
