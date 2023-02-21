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

        var nextTopic = GetNextTopic(users);
        await _updateSlackTopicCommand.Execute(nextTopic);
    }

    private static string GetNextTopic(SlackUser[] users)
    {
        var orderedUsers = users.OrderBy(u => u.userId).ToArray();

        if (orderedUsers.Length == 0)
            return "No users found";

        var selectedUser = orderedUsers[0];

        return $"Next user: {$"<@{selectedUser.userId}>"}";
    }
}
