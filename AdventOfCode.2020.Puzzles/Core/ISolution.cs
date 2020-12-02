using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode._2020.Puzzles.Core
{
    public interface ISolution
    {
        event EventHandler<SolutionProgressEventArgs> ProgressUpdated;

        CancellationToken CancellationToken { get; set; }

        int MillisecondsBetweenProgressUpdates { get; set; }

        Task<string> Part1Async(string input);

        Task<string> Part2Async(string input);
    }
}