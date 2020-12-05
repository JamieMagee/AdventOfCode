using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public interface ISolution
    {
        CancellationToken CancellationToken { get; set; }

        int MillisecondsBetweenProgressUpdates { get; set; }
        event EventHandler<SolutionProgressEventArgs> ProgressUpdated;

        Task<string> Part1Async(string input);

        Task<string> Part2Async(string input);
    }
}