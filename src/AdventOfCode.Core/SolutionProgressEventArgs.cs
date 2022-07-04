namespace AdventOfCode.Core;

public sealed class SolutionProgressEventArgs : EventArgs
{
    public SolutionProgressEventArgs(SolutionProgress solutionProgress) => this.Progress = solutionProgress;

    public SolutionProgress Progress { get; }
}
