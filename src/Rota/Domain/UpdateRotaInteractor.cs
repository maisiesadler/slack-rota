using System.Text.RegularExpressions;
using Rota.Domain.Commands;
using Rota.Domain.Queries;

namespace Rota.Domain;

public class UpdateRotaInteractor
{
    private readonly IGetCurrentSlackTopicQuery _getCurrentSlackTopicQuery;
    private readonly IGetSlackUsersQuery _getSlackUsersQuery;
    private readonly IUpdateSlackTopicCommand _updateSlackTopicCommand;

    public UpdateRotaInteractor(
        IGetCurrentSlackTopicQuery getCurrentSlackTopicQuery,
        IGetSlackUsersQuery getSlackUsersQuery,
        IUpdateSlackTopicCommand updateSlackTopicCommand)
    {
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

    private static string GetNextTopic(string currentTopic, SlackUser[] users)
    {
        var currentUser = GetCurrentUser(currentTopic);

        if (users.Length == 0)
            return "No users found";

        var orderedUsers = users.OrderBy(u => u.userId).ToArray();

        var nextUserIndex = GetNextUserIndex(currentUser, orderedUsers);
        var selectedUser = orderedUsers[nextUserIndex];

        return $"Next user: {$"<@{selectedUser.userId}>"}";
    }

    private static string? GetCurrentUser(string currentTopic)
    {
        var match = Regex.Match(currentTopic, @"\<@([\d\w]+)\>", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        if (!match.Success) return null;

        return match.Groups[1].Value;
    }

    private static int GetNextUserIndex(string? currentUser, SlackUser[] orderedUsers)
    {
        if (currentUser == null) return 0;

        for (int i = 0; i < orderedUsers.Length; i++)
        {
            if (orderedUsers[i].userId == currentUser) return (i + 1) % orderedUsers.Length;
        }

        return 0;
    }
}
