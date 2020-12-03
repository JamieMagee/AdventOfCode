using AdventOfCode._2020.Puzzles.Core;

namespace AdventOfCode._2020.Web.Visualizers
{
    public interface IVisualizer
    {
        ISolution SolutionInstance { get; set; }
    }
}