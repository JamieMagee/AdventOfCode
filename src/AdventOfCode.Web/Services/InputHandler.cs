namespace AdventOfCode.Web.Services;

public sealed class InputHandler : IInputHandler
{
    private readonly HttpClient myHttpClient;
    private readonly Dictionary<int, string> myInputCache = new();
    private readonly Dictionary<int, object[]> myResultCache = new();
    private readonly Dictionary<int, string> mySourceCodeCache = new();

    public InputHandler(HttpClient httpClient) => this.myHttpClient = httpClient;

    public bool IsCachedInputAvailable(int day) => this.myInputCache.ContainsKey(day);

    public async Task<string> GetInputAsync(int year, int day)
    {
        var input = await this.GetFileAsync(year, day, "input/{0}/day{1}.txt", this.myInputCache);
        return input ?? string.Empty;
    }

    public async Task<string> GetSourceCodeAsync(int year, int day)
    {
        var source = await this.GetFileAsync(year, day, "source/{0}/Day{1}.cs", this.mySourceCodeCache);
        return source ?? "No source file available.";
    }

    public object[] GetResults(int day)
    {
        if (!this.myResultCache.TryGetValue(day, out var results))
        {
            results = new object[2];
            this.myResultCache.Add(day, results);
        }

        return results;
    }

    public void ClearResults(int day)
    {
        var results = this.GetResults(day);
        for (var i = 0; i < results.Length; i++)
        {
            results[i] = null!;
        }
    }

    private async Task<string?> GetFileAsync(int year, int day, string pathTemplate, Dictionary<int, string> cache)
    {
        if (!cache.TryGetValue(day, out var description))
        {
            var dayString = day.ToString().PadLeft(2, '0');
            try
            {
                description = await this.myHttpClient.GetStringAsync(string.Format(pathTemplate, year, dayString));
            }
            catch (HttpRequestException)
            {
                return null;
            }

            cache.Add(day, description);
        }

        return description;
    }
}
