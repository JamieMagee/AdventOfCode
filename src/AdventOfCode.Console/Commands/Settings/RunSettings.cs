namespace AdventOfCode.Console.Commands.Settings;

using System.ComponentModel;
using Spectre.Console.Cli;

internal sealed class RunSettings : BaseSettings
{
    [CommandOption("-d|--day")]
    [Description("The specific day to run")]
    public int? Day { get; set; }

    [CommandOption("-a|--all")]
    [Description("Run all available solutions")]
    [DefaultValue(false)]
    public bool All { get; set; }

    [CommandOption("-l|--last")]
    [Description("Run the last available solution")]
    [DefaultValue(false)]
    public bool Last { get; set; }
}
