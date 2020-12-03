using System;

namespace AdventOfCode._2020.Puzzles.Core
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