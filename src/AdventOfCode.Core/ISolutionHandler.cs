namespace AdventOfCode.Core;

public interface ISolutionHandler
{
    IDictionary<int, IDictionary<int, SolutionMetadata>> Solutions { get; }
}
