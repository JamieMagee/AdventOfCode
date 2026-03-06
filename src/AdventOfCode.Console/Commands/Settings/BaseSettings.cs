namespace AdventOfCode.Console.Commands.Settings;

using System.ComponentModel;
using Spectre.Console.Cli;

internal class BaseSettings : CommandSettings
{
    [CommandOption("-y|--year")]
    [Description("The year to run solutions for")]
    public int? Year { get; set; }
}
