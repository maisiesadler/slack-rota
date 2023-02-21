using Rota.Domain.Commands;
using Rota.Domain.Queries;

namespace Rota.Domain;

public class UpdateRotaInteractor
{
    private readonly IGetCurrentSlackTopicQuery _getCurrentSlackTopicQuery;
    private readonly IUpdateSlackTopicCommand _updateSlackTopicCommand;

    public UpdateRotaInteractor(
        IGetCurrentSlackTopicQuery getCurrentSlackTopicQuery,
        IUpdateSlackTopicCommand updateSlackTopicCommand)
    {
        _getCurrentSlackTopicQuery = getCurrentSlackTopicQuery;
        _updateSlackTopicCommand = updateSlackTopicCommand;
    }

    public async Task Execute()
    {
        var topic = await _getCurrentSlackTopicQuery.Execute();;
        await _updateSlackTopicCommand.Execute(topic + "d");
    }
}
