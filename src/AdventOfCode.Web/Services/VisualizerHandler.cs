using AdventOfCode.Core;
using AdventOfCode.Web.Visualizers;

namespace AdventOfCode.Web.Services;

public sealed class VisualizerHandler : IVisualizerHandler
{
    private CancellationTokenSource myCancellationTokenSource = new();

    public VisualizerHandler() => this.VisualizersBySolutionType = GatherPuzzleSolutions();

    public IReadOnlyDictionary<Type, Type> VisualizersBySolutionType { get; }

    public Type GetVisualizer(Type solutionType) => this.VisualizersBySolutionType.TryGetValue(solutionType, out var visualizerType) ? visualizerType : null;

    public void CancelAllVisualizations()
    {
        this.myCancellationTokenSource.Cancel();
        this.myCancellationTokenSource = new CancellationTokenSource();
    }

    public CancellationToken GetVisualizationCancellationToken() => this.myCancellationTokenSource.Token;

    private static Dictionary<Type, Type> GatherPuzzleSolutions()
    {
        var visualizersBySolutionType = new Dictionary<Type, Type>();
        var visualizerInterface = typeof(IVisualizer);
        AssemblyLoader.LoadAssemblies();
        var visualizerTypes = visualizerInterface.Assembly.GetTypes()
            .Where(x => visualizerInterface.IsAssignableFrom(x) && !x.IsAbstract)
            .ToList();

        foreach (var visualizerType in visualizerTypes)
        {
            var targetSolutionAttributes = visualizerType
                .GetCustomAttributes(typeof(TargetSolutionAttribute), false).OfType<TargetSolutionAttribute>()
                .ToList();
            foreach (var targetSolutionType in targetSolutionAttributes.Select(x => x.TargetSolutionType))
            {
                visualizersBySolutionType[targetSolutionType] = visualizerType;
            }
        }

        return visualizersBySolutionType;
    }
}
