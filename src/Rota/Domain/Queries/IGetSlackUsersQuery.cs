namespace Rota.Domain.Queries;

public record SlackUser(string userId);

public interface IGetSlackUsersQuery
{
    Task<SlackUser[]> Execute();
}
