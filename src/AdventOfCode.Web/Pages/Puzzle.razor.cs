using System.Diagnostics;
using AdventOfCode.Core;
using AdventOfCode.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AdventOfCode.Web.Pages;

public sealed partial class Puzzle : IDisposable
{
    private CancellationTokenSource? myCancellationTokenSource;

    private int myProgressRenderTick = Environment.TickCount;

    [Parameter]
    public string Year { get; set; } = (DateTime.Now.Month == 12 ? DateTime.Now.Year : DateTime.Now.Year - 1).ToString();

    [Parameter]
    public string? Day { get; set; }

    [Parameter]
    public int MillisBetweenProgressRender { get; set; } = 100;

    [Inject]
    private ISolutionHandler SolutionHandler { get; set; } = null!;

    [Inject]
    private IInputHandler InputHandler { get; set; } = null!;

    [Inject]
    private IVisualizerHandler VisualizerHandler { get; set; } = null!;

    private SolutionMetadata? SolutionMetadata { get; set; }

    private string? Input { get; set; }

    private string? SourceCode { get; set; }

    private object[]? Results { get; set; }

    private bool IsWorking { get; set; }

    private Stopwatch? CalculationStopwatch { get; set; }

    private bool HasInputChanged { get; set; }

    private SolutionProgress Progress { get; set; } = new();

    private ISolution? SolutionInstance { get; set; }

    private TimeSpan[]? PartTimings { get; set; }

    private Stopwatch[]? PartStopwatches { get; set; }

    protected override Task OnParametersSetAsync() => this.InitAsync();

    private async Task InitAsync()
    {
        Cancel();
        ResetState();

        if (TryParseParameters(out var yearNumber, out var dayNumber) &&
            SolutionHandler.Solutions[yearNumber].TryGetValue(dayNumber, out var solutionMetadata))
        {
            SolutionMetadata = solutionMetadata;
            Results = InputHandler.GetResults(SolutionMetadata.Day);

            if (InputHandler.IsCachedInputAvailable(solutionMetadata.Day))
            {
                await LoadInputAsync();
            }

            LoadPuzzleMetadataInBackground();
        }
    }

    private void ResetState()
    {
        SolutionMetadata = null;
        Input = null;
        HasInputChanged = false;
        Results = null;
        Progress = new SolutionProgress();
        CalculationStopwatch = null;
        SolutionInstance = null;
        PartTimings = null;
        PartStopwatches = null;
    }

    private bool TryParseParameters(out int yearNumber, out int dayNumber)
    {
        yearNumber = 0;
        dayNumber = 0;
        return int.TryParse(Year, out yearNumber) && int.TryParse(Day, out dayNumber);
    }

    private void LoadPuzzleMetadataInBackground()
    {
        if (SolutionMetadata == null) return;

        myCancellationTokenSource = new CancellationTokenSource();

        _ = Task.Run(() => LoadInputAsync(), myCancellationTokenSource.Token);
        _ = Task.Run(async () =>
        {
            if (SolutionMetadata != null)
            {
                SourceCode = await InputHandler.GetSourceCodeAsync(SolutionMetadata.Year, SolutionMetadata.Day);
                StateHasChanged();
            }
        }, myCancellationTokenSource.Token);
    }

    private async Task LoadInputAsync(bool forceReload = false)
    {
        if (SolutionMetadata == null) return;

        Input = forceReload ? null : Input;
        Input ??= await InputHandler.GetInputAsync(SolutionMetadata.Year, SolutionMetadata.Day);
        HasInputChanged = false;
        StateHasChanged();
    }

    private async Task SolveAsync()
    {
        if (SolutionMetadata == null) return;

        myCancellationTokenSource = new CancellationTokenSource();
        SolutionInstance = null;

        try
        {
            IsWorking = true;
            InputHandler.ClearResults(SolutionMetadata.Day);
            Results = InputHandler.GetResults(SolutionMetadata.Day);

            SolutionInstance = CreateAndConfigureSolution();
            CalculationStopwatch = Stopwatch.StartNew();
            PartTimings = new TimeSpan[2];
            PartStopwatches = new Stopwatch[2];
            liveTimerUpdate = CreateLiveTimerUpdate();

            var solutionParts = new Func<string, Task<string>>[]
            {
                SolutionInstance.Part1Async,
                SolutionInstance.Part2Async,
            };

            for (int i = 0; i < solutionParts.Length; i++)
            {
                if (!IsWorking) break;

                Progress = new SolutionProgress();
                StateHasChanged();
                await Task.Delay(1);

                PartStopwatches[i] = Stopwatch.StartNew();
                Results[i] = await ExecuteSolutionPartAsync(solutionParts[i]);
                PartStopwatches[i].Stop();
                PartTimings[i] = PartStopwatches[i].Elapsed;

                // Update UI after each part completes
                StateHasChanged();
            }
        }
        finally
        {
            CleanupSolution();
        }
    }

    private ISolution CreateAndConfigureSolution()
    {
        var solution = SolutionMetadata!.CreateInstance();
        solution.MillisecondsBetweenProgressUpdates = MillisBetweenProgressRender / 2;
        solution.CancellationToken = myCancellationTokenSource!.Token;
        solution.ProgressUpdated += OnProgressUpdate;
        return solution;
    }

    private void CleanupSolution()
    {
        if (SolutionInstance != null)
        {
            SolutionInstance.ProgressUpdated -= OnProgressUpdate;
        }

        liveTimerUpdate?.Dispose();
        liveTimerUpdate = null;
        IsWorking = false;
        CalculationStopwatch?.Stop();
    }

    private async Task<object> ExecuteSolutionPartAsync(Func<string, Task<string>> solutionPart)
    {
        try
        {
            return await (solutionPart(Input ?? string.Empty) ?? Task.FromResult(string.Empty));
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    private void Cancel()
    {
        IsWorking = false;
        liveTimerUpdate?.Dispose();
        liveTimerUpdate = null;
        myCancellationTokenSource?.Cancel();
        VisualizerHandler.CancelAllVisualizations();
    }

    private void OnProgressUpdate(object? sender, SolutionProgressEventArgs args)
    {
        if (Environment.TickCount > myProgressRenderTick)
        {
            Progress = args.Progress;
            StateHasChanged();
            myProgressRenderTick = Environment.TickCount + MillisBetweenProgressRender;
        }
    }

    private Timer? liveTimerUpdate;
    private Timer CreateLiveTimerUpdate()
    {
        return new Timer(_ => _ = InvokeAsync(StateHasChanged), null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));
    }

    private static string FormatElapsedTime(TimeSpan elapsed)
    {
        if (elapsed.TotalMilliseconds < 1000)
        {
            return $"{elapsed.TotalMilliseconds:F0}ms";
        }
        else if (elapsed.TotalSeconds < 60)
        {
            return $"{elapsed.TotalSeconds:F2}s";
        }
        else if (elapsed.TotalMinutes < 60)
        {
            return $"{elapsed.Minutes}m {elapsed.Seconds}s";
        }
        else
        {
            return $"{elapsed.Hours}h {elapsed.Minutes}m {elapsed.Seconds}s";
        }
    }

    private TimeSpan GetCurrentPartElapsed(int partIndex)
    {
        if (PartStopwatches == null || partIndex >= PartStopwatches.Length)
            return TimeSpan.Zero;

        // If the part is already completed, return its timing
        if (PartTimings != null && partIndex < PartTimings.Length && PartTimings[partIndex] != TimeSpan.Zero)
            return PartTimings[partIndex];

        // If this is the currently running part, return its current elapsed time
        if (ShouldShowProgressBar(partIndex) && PartStopwatches[partIndex] != null && PartStopwatches[partIndex].IsRunning)
        {
            return PartStopwatches[partIndex].Elapsed;
        }

        return TimeSpan.Zero;
    }

    public void Dispose()
    {
        Cancel();
        myCancellationTokenSource?.Dispose();
        liveTimerUpdate?.Dispose();
    }
}
