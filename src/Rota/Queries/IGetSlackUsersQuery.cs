namespace Rota.Queries;

public record SlackUser(string userId);

public interface IGetSlackUsersQuery
{
    Task<SlackUser[]> Execute();
}
