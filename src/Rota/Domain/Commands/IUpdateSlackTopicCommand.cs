namespace Rota.Domain.Commands;

public interface IUpdateSlackTopicCommand
{
    Task Execute(string channel, string topic);
}
