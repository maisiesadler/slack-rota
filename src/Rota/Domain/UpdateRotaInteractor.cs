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
        var topic = await _getCurrentSlackTopicQuery.Execute(); ;
        var users = await _getSlackUsersQuery.Execute();
        if (users != null && users.Length > 0)
            await _updateSlackTopicCommand.Execute($"Available: {string.Join(", ", users.Select(u => $"<@{u.userId}>"))}");
        else
            await _updateSlackTopicCommand.Execute(topic + "d");
    }
}
