namespace AdventOfCode.Console.Commands.Settings;

using Spectre.Console.Cli;
using System.ComponentModel;

internal class BaseSettings : CommandSettings
{
    [CommandOption("-y|--year")]
    [Description("The year to run solutions for")]
    public int? Year { get; set; }
}
