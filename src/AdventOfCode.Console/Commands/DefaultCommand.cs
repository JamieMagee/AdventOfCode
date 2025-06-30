namespace AdventOfCode.Console.Commands;

using AdventOfCode.Console.Commands.Settings;
using AdventOfCode.Console.Services;
using AdventOfCode.Core;
using Spectre.Console;
using Spectre.Console.Cli;

internal class DefaultCommand : AsyncCommand<BaseSettings>
{
    private readonly SolutionRunner _solutionRunner;

    public DefaultCommand()
    {
        var config = Configuration.Load();
        var solutionHandler = new SolutionHandler();
        this._solutionRunner = new SolutionRunner(config, solutionHandler);
    }

    public override async Task<int> ExecuteAsync(CommandContext context, BaseSettings settings)
    {
        var solutionHandler = new SolutionHandler();

        // Show welcome message
        var figlet = new FigletText("Advent of Code")
            .LeftJustified()
            .Color(Color.Green);
        AnsiConsole.Write(figlet);

        AnsiConsole.MarkupLine("[dim]Running last available solution. Use --help for available options.[/]");
        AnsiConsole.WriteLine();

        // Determine the year to use
        var year = settings.Year ?? solutionHandler.Solutions.Keys.Max();

        // Run the last available solution
        await this._solutionRunner.RunLastAsync(year);

        return 0;
    }
}
