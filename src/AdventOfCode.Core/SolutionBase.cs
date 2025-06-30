namespace AdventOfCode.Core;

using System.Runtime.CompilerServices;

public abstract class SolutionBase : ISolution, IProgressPublisher
{
    /// <summary>
    ///     A scheduled tick from <see cref="Environment.TickCount" />, when a progress update should happen.
    /// </summary>
    private int myUpdateTick = Environment.TickCount;

    public event EventHandler<SolutionProgressEventArgs>? ProgressUpdated;

    public int MillisecondsBetweenProgressUpdates { get; set; } = 200;

    public CancellationToken CancellationToken { get; set; }

    protected virtual SolutionProgress Progress { get; set; } = new SolutionProgress();

    bool IProgressPublisher.IsUpdateProgressNeeded() => this.IsUpdateProgressNeeded();

    Task IProgressPublisher.UpdateProgressAsync(double current, double total) => this.UpdateProgressAsync(current, total);

    public virtual Task<string> Part1Async(string input) => Task.FromResult(this.Part1(input));

    public virtual Task<string> Part2Async(string input) => Task.FromResult(this.Part2(input));

    /// <summary>
    ///     Breaks the input into lines and removes empty lines at the end.
    /// </summary>
    /// <param name="input">The input string to process.</param>
    protected static IEnumerable<string> GetLines(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return [..input.Replace("\r", string.Empty, StringComparison.Ordinal)
            .Split('\n')
            .Reverse()
            .SkipWhile(string.IsNullOrEmpty)
            .Reverse()];
    }

    protected virtual string Part1(string input) => throw new NotImplementedException();

    protected virtual string Part2(string input) => throw new NotImplementedException();

    /// <summary>
    ///     Returns true if <see cref="UpdateProgressAsync(double, double)" /> should be called to update the UI of the solution runner. This
    ///     happens every couple of milliseconds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsUpdateProgressNeeded() => Environment.TickCount >= this.myUpdateTick;

    /// <summary>
    ///     Updates the UI of the solution runner with the current progress, and schedules the next update a couple of
    ///     milliseconds in the future.
    /// </summary>
    /// <param name="current">The current progress value.</param>
    /// <param name="total">The total value to calculate the percentage from.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected Task UpdateProgressAsync(double current, double total)
    {
        this.Progress.Percentage = current / Math.Max(total, double.Epsilon) * 100;
        return this.UpdateProgressAsync();
    }

    /// <summary>
    ///     Updates the UI of the solution runner with the current progress, and schedules the next update a couple of
    ///     milliseconds in the future.
    /// </summary>
    protected Task UpdateProgressAsync()
    {
        this.myUpdateTick = Environment.TickCount + this.MillisecondsBetweenProgressUpdates;
        this.ProgressUpdated?.Invoke(this, new SolutionProgressEventArgs(this.Progress));
        return Task.Delay(1, this.CancellationToken);
    }
}
