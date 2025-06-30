namespace AdventOfCode.Core;

public interface ISolution
{
    public event EventHandler<SolutionProgressEventArgs> ProgressUpdated;

    public CancellationToken CancellationToken { get; set; }

    public int MillisecondsBetweenProgressUpdates { get; set; }

    public Task<string> Part1Async(string input);

    public Task<string> Part2Async(string input);
}
