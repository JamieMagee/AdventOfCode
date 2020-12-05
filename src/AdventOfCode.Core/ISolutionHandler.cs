using System.Collections.Generic;

namespace AdventOfCode.Core
{
    public interface ISolutionHandler
    {
        IReadOnlyDictionary<int, SolutionMetadata> Solutions { get; }
    }
}