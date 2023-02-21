using Rota.Domain.Commands;

namespace Rota.Domain;

public class UpdateRotaInteractor
{
    private readonly IUpdateRotaCommand _updateRotaCommand;

    public UpdateRotaInteractor(IUpdateRotaCommand updateRotaCommand)
    {
        _updateRotaCommand = updateRotaCommand;
    }

    public async Task Execute()
    {
        await _updateRotaCommand.Execute();
    }
}
