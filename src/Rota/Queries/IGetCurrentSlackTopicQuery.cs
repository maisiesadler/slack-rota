namespace Rota.Queries;

public interface IGetCurrentSlackTopicQuery
{
    Task<string> Execute();
}
