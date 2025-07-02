namespace AdventOfCode.Console.Services;

using AdventOfCode.Core;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;

internal sealed class SolutionRunner
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
        if (!_solutionHandler.Solutions.TryGetValue(year, out IDictionary<int, SolutionMetadata>? value) ||
            !value.TryGetValue(day, out SolutionMetadata? solutionMetadata))
        {
            AnsiConsole.MarkupLine($"[red]No solution found for {year} Day {day}[/]");
            return;
        }

        var solution = solutionMetadata.CreateInstance();

        var dayString = day.ToString().PadLeft(2, '0');
        var rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var inputFile = Path.Combine(rootDir!, "Input", year.ToString(), $"day{dayString}.txt");

        if (!File.Exists(inputFile))
        {
            AnsiConsole.MarkupLine($"[red]Input file not found: {inputFile}[/]");
            return;
        }

        var input = await File.ReadAllTextAsync(inputFile);

        // Run Part 1
        var (result1, time1) = await RunPartAsync(input, solution.Part1Async, solution);

        // Run Part 2
        var (result2, time2) = await RunPartAsync(input, solution.Part2Async, solution);

        // Create a compact grid layout combining title and results
        var grid = new Grid();

        // Add columns: Day/Part, Result, and Time
        grid.AddColumn(new GridColumn().Width(12));              // Day/Part column
        grid.AddColumn(new GridColumn());                        // Result column (expandable)
        grid.AddColumn(new GridColumn().Width(10).PadLeft(1));   // Time column

        // Add day title row spanning across columns
        grid.AddRow(
            new Markup($"[bold blue]Day {day}[/]"),
            new Markup($"[bold blue]{solutionMetadata.Title}[/]"),
            Text.Empty
        );

        // Add separator row
        grid.AddRow(
            new Markup("[dim]────────────[/]"),
            new Markup("[dim]────────────────────────────────────────[/]"),
            new Markup("[dim]──────────[/]")
        );

        // Add Part 1 results
        var part1Result = result1.Contains(Environment.NewLine) ?
            $"[dim]{result1.Trim()}[/]" : result1;

        grid.AddRow(
            new Markup("[yellow]Part 1[/]"),
            new Markup(part1Result),
            new Markup($"[green]{time1}ms[/]")
        );

        // Add Part 2 results
        var part2Result = result2.Contains(Environment.NewLine) ?
            $"[dim]{result2.Trim()}[/]" : result2;

        grid.AddRow(
            new Markup("[yellow]Part 2[/]"),
            new Markup(part2Result),
            new Markup($"[green]{time2}ms[/]")
        );

        // Display the grid directly without border
        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    private static async Task<(string result, long time)> RunPartAsync(
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
