namespace Rota.Test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var slackClient = new SlackHttpClient(new HttpClient());
        await slackClient.Execute("hello, world");
    }
}
