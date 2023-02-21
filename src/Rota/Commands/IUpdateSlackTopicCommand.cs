namespace Rota.Commands;

public interface IUpdateSlackTopicCommand
{
    Task Execute(string topic);
}
