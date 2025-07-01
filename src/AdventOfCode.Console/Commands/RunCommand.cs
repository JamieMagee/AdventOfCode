namespace AdventOfCode.Console.Commands;

using AdventOfCode.Console.Commands.Settings;
using AdventOfCode.Console.Services;
using AdventOfCode.Core;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class RunCommand : AsyncCommand<RunSettings>
{
    private readonly SolutionRunner _solutionRunner;

    public RunCommand()
    {
        var config = Configuration.Load();
        var solutionHandler = new SolutionHandler();
        this._solutionRunner = new SolutionRunner(config, solutionHandler);
    }

    public override async Task<int> ExecuteAsync(CommandContext context, RunSettings settings)
    {
        var solutionHandler = new SolutionHandler();

        // Show welcome message
        var figlet = new FigletText("Advent of Code")
            .LeftJustified()
            .Color(Color.Green);
        AnsiConsole.Write(figlet);

        // Determine the year to use
        var year = settings.Year ?? solutionHandler.Solutions.Keys.Max();

        if (settings.All)
        {
            await this._solutionRunner.RunAllAsync(settings.Year);
        }
        else if (settings.Last)
        {
            await this._solutionRunner.RunLastAsync(year);
        }
        else if (settings.Day.HasValue)
        {
            await this._solutionRunner.RunDayAsync(year, settings.Day.Value);
        }
        else
        {
            // Default behavior: run last day
            await this._solutionRunner.RunLastAsync(year);
        }

        return 0;
    }
}
