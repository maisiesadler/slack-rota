using Rota.Domain.Commands;
using Rota.Infrastructure.Dependencies.Slack;

namespace Rota.Domain;

public class UpdateRotaInteractor
{
    private readonly IUpdateSlackTopicCommand _updateRotaCommand;

    public UpdateRotaInteractor(IUpdateSlackTopicCommand updateRotaCommand)
    {
        _updateRotaCommand = updateRotaCommand;
    }

    public async Task Execute()
    {
        await _updateRotaCommand.Execute("hello, world");
    }
}
