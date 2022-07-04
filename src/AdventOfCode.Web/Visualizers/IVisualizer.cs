using AdventOfCode.Core;

namespace AdventOfCode.Web.Visualizers;

public interface IVisualizer
{
    ISolution SolutionInstance { get; set; }
}
