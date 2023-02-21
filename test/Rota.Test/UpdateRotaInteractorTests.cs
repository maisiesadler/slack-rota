using Microsoft.Extensions.Options;
using Moq;
using Rota.Domain;
using Rota.Domain.Commands;
using Rota.Domain.Queries;

namespace Rota.Test;

public class UpdateRotaInteractorTests
{
    private readonly UpdateRotaInteractor _interactor;
    private readonly Mock<IOptions<RotaOptions>> _options;
    private readonly Mock<IGetCurrentSlackTopicQuery> _getCurrentSlackTopicQuery;
    private readonly Mock<IGetSlackUsersQuery> _getSlackUsersQuery;
    private readonly Mock<IUpdateSlackTopicCommand> _updateSlackTopicCommand;

    public UpdateRotaInteractorTests()
    {
        _options = new Mock<IOptions<RotaOptions>>();
        _getCurrentSlackTopicQuery = new Mock<IGetCurrentSlackTopicQuery>();
        _getSlackUsersQuery = new Mock<IGetSlackUsersQuery>();
        _updateSlackTopicCommand = new Mock<IUpdateSlackTopicCommand>();

        _interactor = new UpdateRotaInteractor(
            _options.Object,
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

    [Theory]
    [InlineData("U123456", "U234567")]
    [InlineData("U234567", "U345678")]
    [InlineData("U345678", "U123456")]
    public async Task UserFound_ChooseUserAlphabetically(string current, string next)
    {
        // Arrange
        _getCurrentSlackTopicQuery.Setup(q => q.Execute())
            .ReturnsAsync($"Next user: <@{current}>");
        _getSlackUsersQuery.Setup(q => q.Execute())
            .ReturnsAsync(new[] { new SlackUser("U234567"), new SlackUser("U123456"), new SlackUser("U345678") });

        // Act
        await _interactor.Execute();

        // Assert
        _updateSlackTopicCommand.Verify(c => c.Execute($"Next user: <@{next}>"), Times.Once);
    }


    [Fact]
    public async Task ExcludeUser_Skipped()
    {
        // Arrange
        _options.Setup(o => o.Value).Returns(new RotaOptions { ExcludeUsers = "U234567|U345678" });
        _getCurrentSlackTopicQuery.Setup(q => q.Execute())
            .ReturnsAsync($"Next user: <@U123456>");
        _getSlackUsersQuery.Setup(q => q.Execute())
            .ReturnsAsync(new[] { new SlackUser("U234567"), new SlackUser("U123456"), new SlackUser("U345678"), new SlackUser("U456789") });

        // Act
        await _interactor.Execute();

        // Assert
        _updateSlackTopicCommand.Verify(c => c.Execute($"Next user: <@U456789>"), Times.Once);
    }
}
