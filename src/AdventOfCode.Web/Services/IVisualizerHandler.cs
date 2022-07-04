namespace AdventOfCode.Web.Services;

public interface IVisualizerHandler
{
    Type GetVisualizer(Type solutionType);

    void CancelAllVisualizations();

    CancellationToken GetVisualizationCancellationToken();
}
