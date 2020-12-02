using AdventOfCode._2020.Puzzles.Core;

namespace aoc2020.web.Visualizers
{
    public interface IVisualizer
    {
        ISolution SolutionInstance { get; set; }
    }
}