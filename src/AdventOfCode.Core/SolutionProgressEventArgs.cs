namespace AdventOfCode.Core;

public sealed class SolutionProgressEventArgs(SolutionProgress solutionProgress) : EventArgs
{
    public SolutionProgress Progress { get; } = solutionProgress;
}
