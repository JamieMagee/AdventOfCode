using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Core;
using AdventOfCode._2020.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AdventOfCode._2020.Web.Pages
{
    public sealed partial class Puzzle
    {
        private CancellationTokenSource myCancellationTokenSource;
        private int myProgressRenderTick = Environment.TickCount;

        [Parameter] public string Day { get; set; }

        [Parameter] public int MillisBetweenProgressRender { get; set; } = 100;

        [Inject] private ISolutionHandler SolutionHandler { get; set; }

        [Inject] private IInputHandler InputHandler { get; set; }

        [Inject] private IVisualizerHandler VisualizerHandler { get; set; }

        private SolutionMetadata SolutionMetadata { get; set; }

        private string Input { get; set; }

        private string Description { get; set; }

        private string SourceCode { get; set; }

        private object[] Results { get; set; }

        private bool IsWorking { get; set; }

        private Stopwatch CalculationStopwatch { get; set; }

        private bool HasInputChanged { get; set; }

        private SolutionProgress Progress { get; set; }

        private ISolution SolutionInstance { get; set; }

        protected override Task OnParametersSetAsync()
        {
            return InitAsync();
        }

        private async Task InitAsync()
        {
            Cancel();
            SolutionMetadata = null;
            Input = null;
            HasInputChanged = false;
            Results = null;
            Progress = new SolutionProgress();
            CalculationStopwatch = null;
            SolutionInstance = null;
            if (int.TryParse(Day, out var dayNumber) &&
                SolutionHandler.Solutions.TryGetValue(dayNumber, out var solutionMetadata))
            {
                SolutionMetadata = solutionMetadata;
                Results = InputHandler.GetResults(SolutionMetadata.Day);
                if (InputHandler.IsCachedInputAvailable(solutionMetadata.Day))
                {
                    await LoadInputAsync();
                }

                Description = "Loading description...";
                LoadPuzzleMetadataInBackground();
            }
        }

        private void LoadPuzzleMetadataInBackground()
        {
            myCancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => LoadInputAsync(), myCancellationTokenSource.Token);
            Task.Run(async () =>
            {
                Description = await InputHandler.GetDescriptionAsync(SolutionMetadata.Day);
                StateHasChanged();
            }, myCancellationTokenSource.Token);
            Task.Run(async () =>
            {
                SourceCode = await InputHandler.GetSourceCodeAsync(SolutionMetadata.Day);
                StateHasChanged();
            }, myCancellationTokenSource.Token);
        }

        private async Task LoadInputAsync(bool forceReload = false)
        {
            Input = forceReload ? null : Input;
            Input ??= await InputHandler.GetInputAsync(SolutionMetadata.Day);
            HasInputChanged = false;
            StateHasChanged();
        }

        private async Task SolveAsync()
        {
            myCancellationTokenSource = new CancellationTokenSource();
            SolutionInstance = null;
            try
            {
                IsWorking = true;
                InputHandler.ClearResults(SolutionMetadata.Day);
                SolutionInstance = SolutionMetadata.CreateInstance();
                SolutionInstance.MillisecondsBetweenProgressUpdates = MillisBetweenProgressRender / 2;
                SolutionInstance.CancellationToken = myCancellationTokenSource.Token;
                SolutionInstance.ProgressUpdated += OnProgressUpdate;
                CalculationStopwatch = Stopwatch.StartNew();
                foreach (var (part, index) in new Func<string, Task<string>>[]
                {
                    SolutionInstance.Part1Async, SolutionInstance.Part2Async
                }.Select((x, i) => (x, i)))
                {
                    Progress = new SolutionProgress();
                    StateHasChanged();
                    await Task.Delay(1);
                    if (IsWorking == false)
                    {
                        break;
                    }

                    Results[index] = await ExceptionToResult(part);
                }
            }
            finally
            {
                if (SolutionInstance != null)
                {
                    SolutionInstance.ProgressUpdated -= OnProgressUpdate;
                }

                IsWorking = false;
                CalculationStopwatch?.Stop();
            }
        }

        private void Cancel()
        {
            IsWorking = false;
            myCancellationTokenSource?.Cancel(true);
            VisualizerHandler.CancelAllVisualizations();
        }

        private void OnProgressUpdate(object sender, SolutionProgressEventArgs args)
        {
            if (Environment.TickCount > myProgressRenderTick)
            {
                Progress = args.Progress;
                StateHasChanged();
                myProgressRenderTick = Environment.TickCount + MillisBetweenProgressRender;
            }
        }

        private async Task<object> ExceptionToResult(Func<string, Task<string>> func)
        {
            try
            {
                return await (func(Input) ?? Task.FromResult<string>(null));
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}