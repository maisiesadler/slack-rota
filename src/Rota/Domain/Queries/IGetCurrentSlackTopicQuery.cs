namespace Rota.Domain.Queries;

public interface IGetCurrentSlackTopicQuery
{
    Task<string> Execute();
}
