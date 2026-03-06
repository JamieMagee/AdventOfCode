namespace AdventOfCode.Console.Commands.Settings;

using System.ComponentModel;
using Spectre.Console.Cli;

internal sealed class SetupSettings : BaseSettings
{
    [CommandArgument(0, "<DAY>")]
    [Description("The day to setup")]
    public int Day { get; set; }
}
