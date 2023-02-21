using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Rota.Domain.Commands;
using Rota.Domain.Queries;

namespace Rota.Domain;

public record RotaOptions
{
    public string? ExcludeUsers { get; init; }
}

public class UpdateRotaInteractor
{
    private readonly IOptions<RotaOptions> _rotaOptions;
    private readonly IGetCurrentSlackTopicQuery _getCurrentSlackTopicQuery;
    private readonly IGetSlackUsersQuery _getSlackUsersQuery;
    private readonly IUpdateSlackTopicCommand _updateSlackTopicCommand;

    public UpdateRotaInteractor(
        IOptions<RotaOptions> rotaOptions,
        IGetCurrentSlackTopicQuery getCurrentSlackTopicQuery,
        IGetSlackUsersQuery getSlackUsersQuery,
        IUpdateSlackTopicCommand updateSlackTopicCommand)
    {
        _rotaOptions = rotaOptions;
        _getCurrentSlackTopicQuery = getCurrentSlackTopicQuery;
        _getSlackUsersQuery = getSlackUsersQuery;
        _updateSlackTopicCommand = updateSlackTopicCommand;
    }

    public async Task Execute()
    {
        var currentTopic = await _getCurrentSlackTopicQuery.Execute(); ;
        var users = await _getSlackUsersQuery.Execute();

        var nextTopic = GetNextTopic(currentTopic, users);

        await _updateSlackTopicCommand.Execute(nextTopic);
    }

    private string GetNextTopic(string currentTopic, SlackUser[] users)
    {
        var currentUser = GetCurrentUser(currentTopic);

        if (users.Length == 0)
            return "No users found";

        var orderedUsers = users.OrderBy(u => u.userId).ToArray();

        var currentUserIndex = GetCurrentUserIndex(currentUser, orderedUsers);
        var selectedUser = GetNextUserAfter(currentUserIndex ?? -1, orderedUsers);

        return $"Next user: {$"<@{selectedUser.userId}>"}";
    }

    private static string? GetCurrentUser(string currentTopic)
    {
        var match = Regex.Match(currentTopic, @"\<@([\d\w]+)\>", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        if (!match.Success) return null;

        return match.Groups[1].Value;
    }

    private static int? GetCurrentUserIndex(string? currentUser, SlackUser[] orderedUsers)
    {
        if (currentUser == null) return null;

        for (int i = 0; i < orderedUsers.Length; i++)
        {
            if (orderedUsers[i].userId == currentUser) return i;
        }

        return null;
    }

    private SlackUser GetNextUserAfter(int afterIndex, SlackUser[] orderedUsers)
    {
        var excludedUsers = new HashSet<string>(_rotaOptions.Value?.ExcludeUsers?.Split('|') ?? Array.Empty<string>());
        for (int i = afterIndex + 1; i < orderedUsers.Length; i++)
        {
            var user = orderedUsers[i];
            if (!excludedUsers.Contains(user.userId))
                return user;
        }

        for (int i = 0; i < orderedUsers.Length; i++)
        {
            var user = orderedUsers[i];
            if (!excludedUsers.Contains(user.userId))
                return user;
        }

        throw new InvalidOperationException("Could not find next user");
    }
}
