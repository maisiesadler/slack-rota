using System.Text.Json.Nodes;
using Rota.Queries;
using Rota.Adapters.Commands;
using Rota.Adapters.Dependencies.Slack;

namespace Rota.Adapters.Queries;

public class GetCurrentSlackTopicQuery : IGetCurrentSlackTopicQuery
{
    private readonly SlackHttpClient _slackHttpClient;

    public GetCurrentSlackTopicQuery(SlackHttpClient slackHttpClient)
    {
        _slackHttpClient = slackHttpClient;
    }

    public async Task<string> Execute()
    {
        var channel = SlackReporterConfiguration.Channel;

        var content = await _slackHttpClient.Run(
             async httpClient => await httpClient.GetAsync($"/api/conversations.info?channel={channel}"));

        var json = JsonNode.Parse(content);
        var currentTopic = json?["channel"]?["topic"]?["value"];
        if (currentTopic == null)
            throw new InvalidOperationException("Could not read current topic");

        return currentTopic.ToString();
    }
}
