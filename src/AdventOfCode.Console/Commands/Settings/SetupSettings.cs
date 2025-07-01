namespace AdventOfCode.Console.Commands.Settings;

using Spectre.Console.Cli;
using System.ComponentModel;

internal sealed class SetupSettings : BaseSettings
{
    [CommandArgument(0, "<DAY>")]
    [Description("The day to setup")]
    public int Day { get; set; }
}
