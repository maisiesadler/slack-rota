using System.Net.Http.Json;
using Rota.Domain.Commands;
using Rota.Infrastructure.Dependencies.Slack;

namespace Rota.Infrastructure.Commands;

public class UpdateSlackTopicCommand : IUpdateSlackTopicCommand
{
    private readonly HttpClient _httpClient;

    public UpdateSlackTopicCommand(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Execute(string topic)
    {
        var channel = SlackReporterConfiguration.Channel;
        var payload = new { channel, topic };

        var r = await _httpClient.PostAsJsonAsync("/api/conversations.setTopic", payload);

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
