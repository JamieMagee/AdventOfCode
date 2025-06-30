namespace AdventOfCode.Console.Services;

using AdventOfCode.Core;
using HtmlAgilityPack;
using Spectre.Console;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

internal class DaySetupService
{
    private readonly Configuration _config;

    public DaySetupService(Configuration config)
    {
        this._config = config;
    }

    public async Task SetupDayAsync(int year, int day)
    {
        var dayString = day.ToString().PadLeft(2, '0');
        var consoleProjectBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var puzzleProjectPath = Path.Combine(
            consoleProjectBinPath,
            "..",
            "..",
            "..",
            "..",
            $"{year}",
            $"AdventOfCode.{year}.Puzzles");

        var rule = new Rule($"[bold green]Setting up {year} Day {dayString}[/]");
        AnsiConsole.Write(rule);

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("session", this._config.SessionCookie, "/", "adventofcode.com"));
        using var httpClientHandler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var httpClient = new HttpClient(httpClientHandler);

        await AnsiConsole.Status()
            .StartAsync("Setting up day...", async ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("yellow"));

                ctx.Status("Downloading input...");
                await SaveInputAsync(year, day, dayString, puzzleProjectPath, httpClient);

                ctx.Status("Getting puzzle title...");
                var puzzleTitle = await GetPuzzleTitleAsync(year, day, httpClient);

                ctx.Status("Creating solution files...");
                await CreateSolutionSourceAsync(year, day, dayString, consoleProjectBinPath, puzzleProjectPath, puzzleTitle);
            });

        AnsiConsole.MarkupLine("[green]✓ Setup completed successfully![/]");
    }

    private static async Task SaveInputAsync(
        int year,
        int day,
        string dayString,
        string puzzleProjectPath,
        HttpClient httpClient)
    {
        var inputAddress = $"https://adventofcode.com/{year}/day/{day}/input";
        var inputFile = new FileInfo(Path.Combine(puzzleProjectPath, "Input", $"day{dayString}.txt"));

        var input = await httpClient.GetStringAsync(inputAddress);
        await File.WriteAllTextAsync(inputFile.FullName, input, Encoding.UTF8);
    }

    private static async Task<string> GetPuzzleTitleAsync(
        int year,
        int day,
        HttpClient httpClient)
    {
        var descriptionAddress = $"https://adventofcode.com/{year}/day/{day}";
        var puzzleTitleRegex = new Regex(@"---.*: (?'title'.*) ---");

        var descriptionPageSource = await httpClient.GetStringAsync(descriptionAddress);
        var descriptionPage = new HtmlDocument();
        descriptionPage.LoadHtml(descriptionPageSource);
        var articleNodes = descriptionPage.DocumentNode.SelectNodes("//article[@class='day-desc']");

        var titleNode = articleNodes.First().SelectSingleNode("//h2");
        var puzzleTitle = puzzleTitleRegex.Match(titleNode.InnerText).Groups["title"].Value;

        return puzzleTitle;
    }

    private static async Task CreateSolutionSourceAsync(
        int year,
        int day,
        string dayString,
        string consoleProjectBinPath,
        string puzzleProjectPath,
        string puzzleTitle)
    {
        var solutionSourceFile = new FileInfo(Path.Combine(consoleProjectBinPath, "Template", "Day_DAYSTRING_.cs"));
        var solutionTargetFile = new FileInfo(Path.Combine(puzzleProjectPath, "Solutions", $"Day{dayString}.cs"));
        var testSourceFile = new FileInfo(Path.Combine(consoleProjectBinPath, "Template", "Day_DAYSTRING_Test.cs"));
        var testTargetFile =
            new FileInfo(Path.Combine($"{puzzleProjectPath}.Test", "Solutions", $"Day{dayString}Test.cs"));

        if (solutionTargetFile.Exists)
        {
            AnsiConsole.MarkupLine($"[yellow]⚠ Source file already exists at {solutionTargetFile.FullName}[/]");
        }
        else
        {
            var sourceContent = await File.ReadAllTextAsync(solutionSourceFile.FullName);
            sourceContent = sourceContent
                .Replace("_YEAR_", $"_{year}")
                .Replace("_DAYNUMBER_", day.ToString())
                .Replace("_DAYSTRING_", dayString)
                .Replace("_PUZZLETITLE_", puzzleTitle);
            await File.WriteAllTextAsync(solutionTargetFile.FullName, sourceContent, Encoding.UTF8);
        }

        if (testTargetFile.Exists)
        {
            AnsiConsole.MarkupLine($"[yellow]⚠ Test file already exists at {testTargetFile.FullName}[/]");
        }
        else
        {
            var testContent = await File.ReadAllTextAsync(testSourceFile.FullName);
            testContent = testContent
                .Replace("_YEAR_", $"_{year}")
                .Replace("_DAYSTRING_", dayString);
            await File.WriteAllTextAsync(testTargetFile.FullName, testContent, Encoding.UTF8);
        }
    }
}
