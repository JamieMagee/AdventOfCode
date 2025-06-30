namespace AdventOfCode.Console.Commands.Settings;

using Spectre.Console.Cli;
using System.ComponentModel;

internal class RunSettings : BaseSettings
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
