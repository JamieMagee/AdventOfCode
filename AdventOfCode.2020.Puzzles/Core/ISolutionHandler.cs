using System.Collections.Generic;

namespace AdventOfCode._2020.Puzzles.Core
{
    public interface ISolutionHandler
    {
        IReadOnlyDictionary<int, SolutionMetadata> Solutions { get; }
    }
}