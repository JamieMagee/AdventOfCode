namespace AdventOfCode.Core;

/// <summary>
/// Interface for handling solutions to Advent of Code puzzles.
/// </summary>
public interface ISolutionHandler
{
    public IDictionary<int, IDictionary<int, SolutionMetadata>> Solutions { get; }
}
