using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode._2020.Puzzles.Core
{
    public abstract class SolutionBase : ISolution, IProgressPublisher
    {
        /// <summary>
        ///     A scheduled tick from <see cref="Environment.TickCount" />, when a progress update should happen.
        /// </summary>
        private int _myUpdateTick = Environment.TickCount;

        protected virtual SolutionProgress Progress { get; set; } = new SolutionProgress();

        bool IProgressPublisher.IsUpdateProgressNeeded()
        {
            return IsUpdateProgressNeeded();
        }

        Task IProgressPublisher.UpdateProgressAsync(double current, double total)
        {
            return UpdateProgressAsync(current, total);
        }

        public event EventHandler<SolutionProgressEventArgs> ProgressUpdated;

        public int MillisecondsBetweenProgressUpdates { get; set; } = 200;

        public CancellationToken CancellationToken { get; set; }

        public virtual Task<string> Part1Async(string input)
        {
            return Task.FromResult(Part1(input));
        }

        public virtual Task<string> Part2Async(string input)
        {
            return Task.FromResult(Part2(input));
        }

        protected virtual string Part1(string input)
        {
            throw new NotImplementedException();
        }

        protected virtual string Part2(string input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Breaks the input into lines and removes empty lines at the end.
        /// </summary>
        public static List<string> GetLines(string input)
        {
            return input.Replace("\r", string.Empty).Split('\n').Reverse().SkipWhile(string.IsNullOrEmpty).Reverse()
                .ToList();
        }

        /// <summary>
        ///     Returns true if <see cref="UpdateProgressAsync" /> should be called to update the UI of the solution runner. This
        ///     happens every couple of milliseconds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsUpdateProgressNeeded()
        {
            return Environment.TickCount >= _myUpdateTick;
        }

        /// <summary>
        ///     Updates the UI of the solution runner with the current progress, and schedules the next update a couple of
        ///     milliseconds in the future.
        /// </summary>
        protected Task UpdateProgressAsync(double current, double total)
        {
            Progress.Percentage = current / Math.Max(total, double.Epsilon) * 100;
            return UpdateProgressAsync();
        }

        /// <summary>
        ///     Updates the UI of the solution runner with the current progress, and schedules the next update a couple of
        ///     milliseconds in the future.
        /// </summary>
        protected Task UpdateProgressAsync()
        {
            _myUpdateTick = Environment.TickCount + MillisecondsBetweenProgressUpdates;
            ProgressUpdated?.Invoke(this, new SolutionProgressEventArgs(Progress));
            return Task.Delay(1, CancellationToken);
        }
    }
}