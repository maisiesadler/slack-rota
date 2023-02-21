namespace Rota.Infrastructure.Commands;

public class SlackHttpException : Exception
{
    public SlackHttpException(string errorContent)
    {
        ErrorContent = errorContent;
    }

    public string ErrorContent { get; }

    public override string ToString()
    {
        return $"{GetType().Name}: '{ErrorContent}'";
    }
}

public class SlackHttpClient
{
    private readonly HttpClient _httpClient;

    public SlackHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Run(Func<HttpClient, Task<HttpResponseMessage>> command)
    {
        var r = await command(_httpClient);

        var content = await r.Content.ReadAsStringAsync();
        if (TryGetErrorMessage(content))
        {
            throw new SlackHttpException(content);
        }

        return content;
    }

    // Slack return a 200 status code even when the payload is an error - this is a work-around
    private bool TryGetErrorMessage(string content)
    {
        if (content.Contains("error"))
        {
            return true;
        }
        return false;
    }
}
