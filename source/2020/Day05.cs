namespace AdventOfCode._2020.Puzzles.Solutions;

[Puzzle("Binary Boarding")]
public sealed class Day05 : SolutionBase
{
    protected override string Part1(string input) => ParseInput(input).Max().ToString();

    protected override string Part2(string input)
    {
        var parsedInput = ParseInput(input).ToList();
        return Enumerable.Range(parsedInput.Min(), parsedInput.Max())
            .Except(parsedInput)
            .First()
            .ToString();
    }

    private static IEnumerable<int> ParseInput(string input) => GetLines(input)
        .Select(row => Convert.ToInt32(
            row.Replace('F', '0')
                .Replace('B', '1')
                .Replace('L', '0')
                .Replace('R', '1'),
            2));
}
