using Moq;
using Rota.Domain;
using Rota.Domain.Commands;

namespace Rota.Test;

public class UpdateRotaInteractorTests
{
    [Fact]
    public async Task InteractorUpdatesUser()
    {
        var updateRotaCommand = new Mock<IUpdateSlackTopicCommand>();
        var interactor = new UpdateRotaInteractor(updateRotaCommand.Object);

        await interactor.Execute();

        // Assert
        updateRotaCommand.Verify(c => c.Execute("hello, world"), Times.Once);
    }
}
