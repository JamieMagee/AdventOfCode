namespace AdventOfCode.Console.Services;

using AdventOfCode.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;

internal class SolutionRunner
{
    private readonly Configuration _config;
    private readonly SolutionHandler _solutionHandler;

    public SolutionRunner(Configuration config, SolutionHandler solutionHandler)
    {
        this._config = config;
        this._solutionHandler = solutionHandler;
    }

    public async Task RunAllAsync(int? year = null)
    {
        var rule = new Rule("[bold blue]Running All Solutions[/]");
        AnsiConsole.Write(rule);

        if (year.HasValue)
        {
            await this.RunAllByYearAsync(year.Value);
        }
        else
        {
            foreach (var solutionYear in this._solutionHandler.Solutions.Keys.OrderBy(x => x))
            {
                await this.RunAllByYearAsync(solutionYear);
            }
        }
    }

    public async Task RunAllByYearAsync(int year)
    {
        var panel = new Panel($"[bold yellow]{year}[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Yellow);
        AnsiConsole.Write(panel);

        foreach (var day in this._solutionHandler.Solutions[year].Keys.OrderBy(x => x))
        {
            await this.RunDayAsync(year, day);
        }

        AnsiConsole.WriteLine();
    }

    public async Task RunLastAsync(int year)
    {
        var lastSolutionDay = this._solutionHandler.Solutions[year].Keys
            .Where(x => x is >= 1 and <= 25)
            .MaxBy(x => x);

        if (lastSolutionDay > 0)
        {
            var rule = new Rule($"[bold green]Running Last Solution - {year} Day {lastSolutionDay}[/]");
            AnsiConsole.Write(rule);
            await this.RunDayAsync(year, lastSolutionDay);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No solution is available yet.[/]");
        }
    }

    public async Task RunDayAsync(int year, int day)
    {
        if (!this._solutionHandler.Solutions.ContainsKey(year) ||
            !this._solutionHandler.Solutions[year].ContainsKey(day))
        {
            AnsiConsole.MarkupLine($"[red]No solution found for {year} Day {day}[/]");
            return;
        }

        var solutionMetadata = this._solutionHandler.Solutions[year][day];
        var solution = solutionMetadata.CreateInstance();

        var dayString = day.ToString().PadLeft(2, '0');
        var rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var inputFile = Path.Combine(rootDir, "Input", year.ToString(), $"day{dayString}.txt");

        if (!File.Exists(inputFile))
        {
            AnsiConsole.MarkupLine($"[red]Input file not found: {inputFile}[/]");
            return;
        }

        var input = await File.ReadAllTextAsync(inputFile);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .AddColumn(new TableColumn("[bold]Part[/]").Centered())
            .AddColumn(new TableColumn("[bold]Result[/]").LeftAligned())
            .AddColumn(new TableColumn("[bold]Time[/]").RightAligned());

        AnsiConsole.Write(new Panel($"[bold blue]Day {day}: {solutionMetadata.Title}[/]")
            .Border(BoxBorder.Double)
            .BorderColor(Color.Blue));

        // Run Part 1
        var (result1, time1) = await this.RunPartAsync(input, solution.Part1Async, solution);
        table.AddRow("[yellow]1[/]", result1.Contains(Environment.NewLine) ?
            $"[dim]{Environment.NewLine}{result1}[/]" : result1, $"[green]{time1}ms[/]");

        // Run Part 2
        var (result2, time2) = await this.RunPartAsync(input, solution.Part2Async, solution);
        table.AddRow("[yellow]2[/]", result2.Contains(Environment.NewLine) ?
            $"[dim]{Environment.NewLine}{result2}[/]" : result2, $"[green]{time2}ms[/]");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private async Task<(string result, long time)> RunPartAsync(
        string input,
        Func<string, Task<string>> action,
        ISolution solution)
    {
        var stopwatch = new Stopwatch();
        string result = string.Empty;

        await AnsiConsole.Status()
            .StartAsync("Calculating...", async ctx =>
            {
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));

                stopwatch.Start();
                try
                {
                    solution.ProgressUpdated += (_, args) =>
                    {
                        if (args.Progress.Percentage > 0)
                        {
                            ctx.Status($"Calculating... {Math.Min(99.99, args.Progress.Percentage):0.00}%");
                        }
                    };
                    result = await action(input);
                }
                catch (NotImplementedException)
                {
                    result = "[dim]Not implemented.[/]";
                }
                catch (Exception ex)
                {
                    result = $"[red]({ex.GetType().Name}) {ex.Message}[/]";
                }
                finally
                {
                    stopwatch.Stop();
                }
            });

        return (result, stopwatch.ElapsedMilliseconds);
    }
}
