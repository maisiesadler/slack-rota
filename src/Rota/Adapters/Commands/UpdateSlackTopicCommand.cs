using System.Net.Http.Json;
using Rota.Commands;
using Rota.Adapters.Dependencies.Slack;

namespace Rota.Adapters.Commands;

public class UpdateSlackTopicCommand : IUpdateSlackTopicCommand
{
    private readonly SlackHttpClient _slackHttpClient;

    public UpdateSlackTopicCommand(SlackHttpClient slackHttpClient)
    {
        _slackHttpClient = slackHttpClient;
    }

    public async Task Execute(string topic)
    {
        var channel = SlackReporterConfiguration.Channel;
        var payload = new { channel, topic };

        var content = await _slackHttpClient.Run(
            async httpClient => await httpClient.PostAsJsonAsync("/api/conversations.setTopic", payload));
    }
}
