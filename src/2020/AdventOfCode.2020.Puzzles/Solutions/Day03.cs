namespace AdventOfCode._2020.Puzzles.Solutions;

[Puzzle("Toboggan Trajectory")]
public sealed class Day03 : SolutionBase
{
    private const char Tree = '#';

    private readonly List<(int, int)> day2Slopes = new()
    {
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2),
    };

    protected override string Part1(string input) => CalculateTrees(input, 3, 1).ToString();

    protected override string Part2(string input) => this.day2Slopes.Select(tuple => CalculateTrees(input, tuple.Item1, tuple.Item2))
        .Aggregate(1U, (acc, val) => acc * val)
        .ToString();

    private static uint CalculateTrees(string input, int xIncrement, int yIncrement)
    {
        var parsedInput = ParseInput(input);
        int x = 0, y = 0;
        uint countTrees = 0;
        do
        {
            x = (x + xIncrement) % parsedInput[0].Length;
            y += yIncrement;
            var item = parsedInput[y][x];
            countTrees += item == Tree ? 1u : 0u;
        }
        while (y < parsedInput.Count - 1);

        return countTrees;
    }

    private static IList<char[]> ParseInput(string input) => input.Trim()
        .Split('\n')
        .Select(line => line.ToCharArray())
        .ToList();
}
