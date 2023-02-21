namespace Rota.Adapters.Dependencies.Slack;

internal static class SlackReporterConfiguration
{
    public static string Channel => GetVariable("SLACK_ALERT_CHANNEL");
    public static string Token => GetVariable("SLACK_TOKEN");

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
