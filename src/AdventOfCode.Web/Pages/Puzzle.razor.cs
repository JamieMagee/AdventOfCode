using System.Diagnostics;
using AdventOfCode.Core;
using AdventOfCode.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AdventOfCode.Web.Pages;

public sealed partial class Puzzle
{
    private CancellationTokenSource myCancellationTokenSource;
    private int myProgressRenderTick = Environment.TickCount;

    [Parameter]
    public string Year { get; set; } = (DateTime.Now.Month == 12 ? DateTime.Now.Year : DateTime.Now.Year - 1).ToString();

    [Parameter]
    public string Day { get; set; }

    [Parameter]
    public int MillisBetweenProgressRender { get; set; } = 100;

    [Inject]
    private ISolutionHandler SolutionHandler { get; set; }

    [Inject]
    private IInputHandler InputHandler { get; set; }

    [Inject]
    private IVisualizerHandler VisualizerHandler { get; set; }

    private SolutionMetadata SolutionMetadata { get; set; }

    private string Input { get; set; }

    private string SourceCode { get; set; }

    private object[] Results { get; set; }

    private bool IsWorking { get; set; }

    private Stopwatch CalculationStopwatch { get; set; }

    private bool HasInputChanged { get; set; }

    private SolutionProgress Progress { get; set; }

    private ISolution SolutionInstance { get; set; }

    protected override Task OnParametersSetAsync() => this.InitAsync();

    private async Task InitAsync()
    {
        this.Cancel();
        this.SolutionMetadata = null;
        this.Input = null;
        this.HasInputChanged = false;
        this.Results = null;
        this.Progress = new SolutionProgress();
        this.CalculationStopwatch = null;
        this.SolutionInstance = null;
        if (int.TryParse(this.Year, out var yearNumber) && int.TryParse(this.Day, out var dayNumber) && this.SolutionHandler.Solutions[yearNumber].TryGetValue(dayNumber, out var solutionMetadata))
        {
            this.SolutionMetadata = solutionMetadata;
            this.Results = this.InputHandler.GetResults(this.SolutionMetadata.Day);
            if (this.InputHandler.IsCachedInputAvailable(solutionMetadata.Day))
            {
                await this.LoadInputAsync();
            }

            this.LoadPuzzleMetadataInBackground();
        }
    }

    private void LoadPuzzleMetadataInBackground()
    {
        this.myCancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => this.LoadInputAsync(), this.myCancellationTokenSource.Token);
        Task.Run(
            async () =>
        {
            this.SourceCode = await this.InputHandler.GetSourceCodeAsync(this.SolutionMetadata.Year, this.SolutionMetadata.Day);
            this.StateHasChanged();
        },
            this.myCancellationTokenSource.Token);
    }

    private async Task LoadInputAsync(bool forceReload = false)
    {
        this.Input = forceReload ? null : this.Input;
        this.Input ??= await this.InputHandler.GetInputAsync(this.SolutionMetadata.Year, this.SolutionMetadata.Day);
        this.HasInputChanged = false;
        this.StateHasChanged();
    }

    private async Task SolveAsync()
    {
        this.myCancellationTokenSource = new CancellationTokenSource();
        this.SolutionInstance = null;
        try
        {
            this.IsWorking = true;
            this.InputHandler.ClearResults(this.SolutionMetadata.Day);
            this.SolutionInstance = this.SolutionMetadata.CreateInstance();
            this.SolutionInstance.MillisecondsBetweenProgressUpdates = this.MillisBetweenProgressRender / 2;
            this.SolutionInstance.CancellationToken = this.myCancellationTokenSource.Token;
            this.SolutionInstance.ProgressUpdated += this.OnProgressUpdate;
            this.CalculationStopwatch = Stopwatch.StartNew();
            foreach (var (part, index) in new Func<string, Task<string>>[]
                     {
                         this.SolutionInstance.Part1Async, this.SolutionInstance.Part2Async,
                     }.Select((x, i) => (x, i)))
            {
                this.Progress = new SolutionProgress();
                this.StateHasChanged();
                await Task.Delay(1);
                if (this.IsWorking == false)
                {
                    break;
                }

                this.Results[index] = await this.ExceptionToResultAsync(part);
            }
        }
        finally
        {
            if (this.SolutionInstance != null)
            {
                this.SolutionInstance.ProgressUpdated -= this.OnProgressUpdate;
            }

            this.IsWorking = false;
            this.CalculationStopwatch?.Stop();
        }
    }

    private void Cancel()
    {
        this.IsWorking = false;
        this.myCancellationTokenSource?.Cancel(true);
        this.VisualizerHandler.CancelAllVisualizations();
    }

    private void OnProgressUpdate(object sender, SolutionProgressEventArgs args)
    {
        if (Environment.TickCount > this.myProgressRenderTick)
        {
            this.Progress = args.Progress;
            this.StateHasChanged();
            this.myProgressRenderTick = Environment.TickCount + this.MillisBetweenProgressRender;
        }
    }

    private async Task<object> ExceptionToResultAsync(Func<string, Task<string>> func)
    {
        try
        {
            return await (func(this.Input) ?? Task.FromResult<string>(null));
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
}
