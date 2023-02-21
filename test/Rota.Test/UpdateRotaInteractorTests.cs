using Moq;
using Rota.Domain;
using Rota.Domain.Commands;
using Rota.Domain.Queries;

namespace Rota.Test;

public class UpdateRotaInteractorTests
{
    private readonly UpdateRotaInteractor _interactor;
    private readonly Mock<IGetCurrentSlackTopicQuery> _getCurrentSlackTopicQuery;
    private readonly Mock<IUpdateSlackTopicCommand> _updateSlackTopicCommand;

    public UpdateRotaInteractorTests()
    {
        _getCurrentSlackTopicQuery = new Mock<IGetCurrentSlackTopicQuery>();
        _updateSlackTopicCommand = new Mock<IUpdateSlackTopicCommand>();

        _interactor = new UpdateRotaInteractor(
            _getCurrentSlackTopicQuery.Object,
            _updateSlackTopicCommand.Object);
    }

    [Fact]
    public async Task InteractorUpdatesTopic()
    {
        // Act
        await _interactor.Execute();

        // Assert
        _updateSlackTopicCommand.Verify(c => c.Execute("hello, world"), Times.Once);
    }

    [Fact]
    public async Task InteractorReversesTopic()
    {
        // Arrange
        _getCurrentSlackTopicQuery.Setup(q => q.Execute())
            .ReturnsAsync("This is the current topic");

        // Act
        await _interactor.Execute();

        // Assert
        _getCurrentSlackTopicQuery.Verify(c => c.Execute(), Times.Once);
        _updateSlackTopicCommand.Verify(c => c.Execute("hello, world"), Times.Once);
    }
}
