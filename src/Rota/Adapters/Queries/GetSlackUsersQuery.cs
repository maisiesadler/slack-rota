using System.Text.Json.Nodes;
using Rota.Queries;
using Rota.Adapters.Commands;
using Rota.Adapters.Dependencies.Slack;

namespace Rota.Adapters.Queries;

public class GetSlackUsersQuery : IGetSlackUsersQuery
{
    private readonly SlackHttpClient _slackHttpClient;

    public GetSlackUsersQuery(SlackHttpClient slackHttpClient)
    {
        _slackHttpClient = slackHttpClient;
    }

    public async Task<SlackUser[]> Execute()
    {
        var channel = SlackReporterConfiguration.Channel;

        var content = await _slackHttpClient.Run(
             async httpClient => await httpClient.GetAsync($"/api/conversations.members?channel={channel}"));

        var json = JsonNode.Parse(content);
        var members = json?["members"];
        if (members == null)
            throw new InvalidOperationException("Could not get members");

        return members.AsArray()
            .Select(m => new SlackUser(m!.ToString()))
            .ToArray();
    }
}
