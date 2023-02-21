namespace Rota;

public class SlackReporterConfiguration
{
    public string Channel => GetVariable("SLACK_ALERT_CHANNEL");
    public string Token => GetVariable("SLACK_TOKEN");

    private static string GetVariable(string variable)
    {
        var value = Environment.GetEnvironmentVariable(variable);

        if (string.IsNullOrWhiteSpace(value))
        {
            System.Console.WriteLine($"Environment variable '{variable}' not set.");
        }

        return value ?? throw new ArgumentNullException($"Missing config '{variable}'");
    }
}
