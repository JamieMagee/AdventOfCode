namespace AdventOfCode.Console.Commands;

using AdventOfCode.Console.Commands.Settings;
using AdventOfCode.Console.Services;
using AdventOfCode.Core;
using Spectre.Console.Cli;

internal class SetupCommand : AsyncCommand<SetupSettings>
{
    private readonly DaySetupService _daySetupService;

    public SetupCommand()
    {
        var config = Configuration.Load();
        this._daySetupService = new DaySetupService(config);
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SetupSettings settings)
    {
        var solutionHandler = new SolutionHandler();
        var year = settings.Year ?? solutionHandler.Solutions.Keys.Max();

        await this._daySetupService.SetupDayAsync(year, settings.Day);

        return 0;
    }
}
