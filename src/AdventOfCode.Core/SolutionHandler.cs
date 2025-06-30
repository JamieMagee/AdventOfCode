namespace AdventOfCode.Core;

using System.Globalization;
using System.Text.RegularExpressions;

/// <inheritdoc/>
public partial class SolutionHandler : ISolutionHandler
{
    public SolutionHandler() => this.Solutions = GatherPuzzleSolutions();

    public IDictionary<int, IDictionary<int, SolutionMetadata>> Solutions { get; }

    private static Dictionary<int, IDictionary<int, SolutionMetadata>> GatherPuzzleSolutions()
    {
        var solutionsByYear = new Dictionary<int, IDictionary<int, SolutionMetadata>>();

        AssemblyLoader.LoadAssemblies();
        var solutionTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(ISolution).IsAssignableFrom(x) && !x.IsAbstract)
            .ToList();
        var years = solutionTypes.Select(t => Convert.ToInt32(YearRegex().Match(t.FullName!).Value, CultureInfo.InvariantCulture))
            .Distinct();

        foreach (var year in years)
        {
            var solutionsByDay = new Dictionary<int, SolutionMetadata>();
            foreach (var solutionType in solutionTypes.Where(t => t.FullName!.Contains(year.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal)))
            {
                var puzzleAttribute = solutionType.GetCustomAttributes(typeof(PuzzleAttribute), false)
                    .OfType<PuzzleAttribute>().FirstOrDefault();
                var day = puzzleAttribute?.Day ?? Convert.ToInt32(NumberRegex().Match(solutionType.Name).Value, CultureInfo.InvariantCulture);
                var title = puzzleAttribute?.Title ?? $"Puzzle #{day}";
                solutionsByDay.Add(day, new SolutionMetadata(solutionType, year, day, title));
            }

            solutionsByYear.Add(year, solutionsByDay);
        }

        return solutionsByYear;
    }

    [GeneratedRegex(@"20[0-9]+")]
    private static partial Regex YearRegex();

    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex NumberRegex();
}
