namespace AdventOfCode._2020.Puzzles.Solutions;

[Puzzle("Custom Customs")]
public sealed class Day06 : SolutionBase
{
    protected override string Part1(string input) => ParseInput(input).Select(x => new string(
                [.. x.Replace("\n", string.Empty).Distinct()]))
        .Sum(x => x.Length)
        .ToString();

    protected override string Part2(string input) => ParseInput(input).Select(x =>
            x.Split("\n").Aggregate((a, b) => new string([.. a.Intersect(b)])))
        .Sum(x => x.Length)
        .ToString();

    private static IEnumerable<string> ParseInput(string input) => input.Trim().Split("\n\n");
}
