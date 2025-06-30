namespace AdventOfCode.Console;

using AdventOfCode.Console.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class Program
{
    public static async Task<int> Main(string[] args)
    {
        var app = new CommandApp<DefaultCommand>();

        app.Configure(config =>
        {
            config.SetApplicationName("aoc");
            config.SetApplicationVersion("1.0.0");

            config.AddCommand<RunCommand>("run")
                .WithDescription("Run Advent of Code solutions")
                .WithExample(["run", "--all"])
                .WithExample(["run", "--last"])
                .WithExample(["run", "--day", "12"])
                .WithExample(["run", "--year", "2020", "--day", "5"]);

            config.AddCommand<SetupCommand>("setup")
                .WithDescription("Setup a new day by downloading input and creating solution files")
                .WithExample(["setup", "23"])
                .WithExample(["setup", "15", "--year", "2020"]);

            config.SetExceptionHandler((ex, resolver) =>
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return -1;
            });

#if DEBUG
            config.PropagateExceptions();
            config.ValidateExamples();
#endif
        });

        return await app.RunAsync(args).ConfigureAwait(false);
    }
}
