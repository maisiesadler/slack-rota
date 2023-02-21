using Moq;
using Rota.Domain;
using Rota.Domain.Commands;
using Rota.Domain.Queries;

namespace Rota.Test;

public class UpdateRotaInteractorTests
{
    private readonly UpdateRotaInteractor _interactor;
    private readonly Mock<IGetCurrentSlackTopicQuery> _getCurrentSlackTopicQuery;
    private readonly Mock<IGetSlackUsersQuery> _getSlackUsersQuery;
    private readonly Mock<IUpdateSlackTopicCommand> _updateSlackTopicCommand;

    public UpdateRotaInteractorTests()
    {
        _getCurrentSlackTopicQuery = new Mock<IGetCurrentSlackTopicQuery>();
        _getSlackUsersQuery = new Mock<IGetSlackUsersQuery>();
        _updateSlackTopicCommand = new Mock<IUpdateSlackTopicCommand>();

        _interactor = new UpdateRotaInteractor(
            _getCurrentSlackTopicQuery.Object,
            _getSlackUsersQuery.Object,
            _updateSlackTopicCommand.Object);
    }

    [Fact]
    public async Task NoUsers_SetToEmpty()
    {
        // Arrange
        _getCurrentSlackTopicQuery.Setup(q => q.Execute()).ReturnsAsync("This is the current topic");
        _getSlackUsersQuery.Setup(q => q.Execute()).ReturnsAsync(Array.Empty<SlackUser>());

        // Act
        await _interactor.Execute();

        // Assert
        _updateSlackTopicCommand.Verify(c => c.Execute("No users found"), Times.Once);
    }

    [Fact]
    public async Task CurrentUserNotKnown_ChooseUserAlphabetically()
    {
        // Arrange
        _getCurrentSlackTopicQuery.Setup(q => q.Execute())
            .ReturnsAsync("Current user not known");
        _getSlackUsersQuery.Setup(q => q.Execute())
            .ReturnsAsync(new[] { new SlackUser("U23456"), new SlackUser("U123456"), new SlackUser("U34567") });

        // Act
        await _interactor.Execute();

        // Assert
        _updateSlackTopicCommand.Verify(c => c.Execute("Next user: <@U123456>"), Times.Once);
    }
}
