namespace Rota.Domain.Commands;

public interface IUpdateSlackTopicCommand
{
    Task Execute(string topic);
}
