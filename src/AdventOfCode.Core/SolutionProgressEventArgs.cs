using System;

namespace AdventOfCode.Core
{
    public sealed class SolutionProgressEventArgs : EventArgs
    {
        public readonly SolutionProgress Progress;

        public SolutionProgressEventArgs(SolutionProgress solutionProgress)
        {
            Progress = solutionProgress;
        }
    }
}