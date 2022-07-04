namespace AdventOfCode.Console;

using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using CommandLine;
using CommandLine.Text;
using HtmlAgilityPack;

internal sealed class Program
{
    private readonly Configuration myConfig;
    private readonly Options myOptions;
    private readonly SolutionHandler mySolutionHandler;

    private Program(string[] args)
    {
        Options options = null;
        if (!args.Any())
        {
            System.Console.WriteLine("Running last available solution. Use --help for available options.");
            System.Console.WriteLine();
            options = new Options
            {
                RunLastDay = true,
            };
        }
        else
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);
        }

        this.myConfig = Configuration.Load();
        this.myOptions = options;
        this.mySolutionHandler = new SolutionHandler();
    }

    public static async Task Main(string[] args) => await new Program(args).RunAsync();

    private async Task RunAsync()
    {
        if (this.myOptions == null)
        {
            return;
        }

        if (this.myOptions.DayToSetup.HasValue)
        {
            if (this.myOptions.Year.HasValue)
            {
                await this.SetupDayAsync(this.myOptions.Year.Value, this.myOptions.DayToSetup.Value);
            }
            else
            {
                await this.SetupDayAsync(this.mySolutionHandler.Solutions.Keys.Max(), this.myOptions.DayToSetup.Value);
            }
        }

        if (this.myOptions.RunAllDays)
        {
            if (this.myOptions.Year.HasValue)
            {
                await this.SolveAllDaysByYearAsync(this.myOptions.Year.Value);
            }
            else
            {
                await this.SolveAllDaysAsync();
            }

            return;
        }

        if (this.myOptions.RunLastDay)
        {
            if (this.myOptions.Year.HasValue)
            {
                await this.SolveLastDayAsync(this.myOptions.Year.Value);
            }
            else
            {
                await this.SolveLastDayAsync(this.mySolutionHandler.Solutions.Keys.Max());
            }
        }

        if (this.myOptions.DayToRun.HasValue)
        {
            if (this.myOptions.Year.HasValue)
            {
                await this.SolveDayAsync(this.myOptions.Year.Value, this.myOptions.DayToRun.Value);
            }
            else
            {
                await this.SolveDayAsync(this.mySolutionHandler.Solutions.Keys.Max(), this.myOptions.DayToRun.Value);
            }
        }
    }

    private async Task SolveAllDaysAsync()
    {
        foreach (var year in this.mySolutionHandler.Solutions.Keys)
        {
            await this.SolveAllDaysByYearAsync(year);
        }
    }

    private async Task SolveAllDaysByYearAsync(int year)
    {
        System.Console.WriteLine($"{year}");
        foreach (var day in this.mySolutionHandler.Solutions[year].Keys.OrderBy(x => x))
        {
            await this.SolveDayAsync(year, day);
        }

        System.Console.WriteLine();
    }

    private async Task SolveLastDayAsync(int year)
    {
        var lastSolutionDay = this.mySolutionHandler.Solutions[year].Keys.LastOrDefault(x => x is >= 1 and <= 25);
        if (lastSolutionDay > 0)
        {
            await this.SolveDayAsync(year, lastSolutionDay);
        }
        else
        {
            System.Console.WriteLine("No solution is available yet.");
        }
    }

    private async Task SolveDayAsync(int year, int day)
    {
        var solutionMetadata = this.mySolutionHandler.Solutions[year][day];
        var solution = solutionMetadata.CreateInstance();

        var dayString = day.ToString().PadLeft(2, '0');
        var rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var input = await File.ReadAllTextAsync(Path.Combine(rootDir, "Input", year.ToString(), $"day{dayString}.txt"));

        System.Console.WriteLine($"Day {day}: {solutionMetadata.Title}");
        await SolvePartAsync(1, input, solution.Part1Async, solution);
        await SolvePartAsync(2, input, solution.Part2Async, solution);
    }

    private static async Task SolvePartAsync(
        int partNumber,
        string input,
        Func<string, Task<string>> action,
        ISolution solution)
    {
        var stopwatch = new Stopwatch();
        var emptyWaitingMessage = $"Calculating Part {partNumber}... ";
        var waitingMessage = emptyWaitingMessage;

        System.Console.Write(waitingMessage);

        stopwatch.Start();
        string result;
        try
        {
            solution.ProgressUpdated += ProgressUpdated;
            result = await action(input);
        }
        catch (NotImplementedException)
        {
            result = "Not implemented.";
        }
        catch (Exception ex)
        {
            result = $"({ex.GetType().Name}) {ex.Message}";
        }
        finally
        {
            solution.ProgressUpdated -= ProgressUpdated;
            stopwatch.Stop();
        }

        System.Console.Write($"\r{new string(' ', waitingMessage.Length)}\r");
        System.Console.Write($"Part {partNumber}: ");
        if (result.Contains(Environment.NewLine))
        {
            result = Environment.NewLine + result;
        }

        System.Console.Write(result);
        System.Console.WriteLine($" ({stopwatch.ElapsedMilliseconds}ms)");

        void ProgressUpdated(object obj, SolutionProgressEventArgs args)
        {
            if (args.Progress.Percentage > 0)
            {
                var prevLength = waitingMessage.Length;
                waitingMessage = $"\r{emptyWaitingMessage}{Math.Min(99.99, args.Progress.Percentage):0.00}%";
                System.Console.Write(waitingMessage +
                                     new string(' ', Math.Max(0, prevLength - waitingMessage.Length)));
            }
        }
    }

    private async Task SetupDayAsync(int year, int day)
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
        System.Console.WriteLine($"Setting up input for {year}/{dayString}...");

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("session", this.myConfig.SessionCookie, "/", "adventofcode.com"));
        using var httpClientHandler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var httpClient = new HttpClient(httpClientHandler);

        await SaveInputAsync(year, day, dayString, puzzleProjectPath, httpClient);
        var puzzleTitle = await GetPuzzleTitleAsync(year, day, httpClient);
        await CreateSolutionSourceAsync(year, day, dayString, consoleProjectBinPath, puzzleProjectPath, puzzleTitle);

        System.Console.WriteLine("Done.");
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
        System.Console.WriteLine($"Downloading input from {inputAddress}");
        var input = await httpClient.GetStringAsync(inputAddress);

        System.Console.WriteLine($"Saving input to {inputFile.FullName}");
        await File.WriteAllTextAsync(inputFile.FullName, input, Encoding.UTF8);
    }

    private static async Task<string> GetPuzzleTitleAsync(
        int year,
        int day,
        HttpClient httpClient)
    {
        var descriptionAddress = $"https://adventofcode.com/{year}/day/{day}";
        var puzzleTitleRegex = new Regex(@"---.*: (?'title'.*) ---");

        System.Console.WriteLine($"Downloading description from {descriptionAddress}");
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
            System.Console.WriteLine($"Source file already exists at {solutionTargetFile.FullName}");
        }
        else
        {
            System.Console.WriteLine($"Saving source file to {solutionTargetFile.FullName}");
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
            System.Console.WriteLine($"Test file already exists at {solutionTargetFile.FullName}");
        }
        else
        {
            System.Console.WriteLine($"Saving test file to {testTargetFile.FullName}");
            var testContent = await File.ReadAllTextAsync(testSourceFile.FullName);
            testContent = testContent
                .Replace("_YEAR_", $"_{year}")
                .Replace("_DAYSTRING_", dayString);
            await File.WriteAllTextAsync(testTargetFile.FullName, testContent, Encoding.UTF8);
        }
    }

    private sealed class Options
    {
        [Option('a', "all", HelpText = "Run all available solutions.")]
        public bool RunAllDays { get; private set; }

        [Option('l', "last", HelpText = "Run the last available solution.")]
        public bool RunLastDay { get; set; }

        [Option('y', "year", HelpText = "[Number of year] Run the solutions for the given year.")]
        public int? Year { get; private set; }

        [Option('d', "day", HelpText = "[Number of day] Run the solution for the given day.")]
        public int? DayToRun { get; private set; }

        [Option(
            's',
            "setup",
            HelpText = "[Number of day] Download input for given day, and add it to AdventOfCode._2020.Puzzles along with an empty solution .cs file.")]
        public int? DayToSetup { get; private set; }

        [Usage(ApplicationAlias = "AdventOfCode.Console")]
        public static IEnumerable<Example> Examples => new[]
        {
            new Example("Run all available solutions", new Options { RunAllDays = true }),
            new Example("Run the last available solution", new Options { RunLastDay = true }),
            new Example(
                "Run solution for day 12",
                new UnParserSettings { PreferShortName = true },
                new Options { DayToRun = 12 }),
            new Example(
                "Add input for day 23 to AdventOfCode._2020.Puzzles along with an empty test and solution .cs file",
                new Options { DayToSetup = 23 }),
        };
    }
}
