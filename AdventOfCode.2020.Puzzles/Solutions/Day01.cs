using System.Linq;
using AdventOfCode._2020.Puzzles.Core;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [PuzzleAttribute("Report Repair")]
    public sealed class Day01 : SolutionBase
    {
        protected override string Part1(string input)
        {
            return Solve(input, 2);
        }

        protected override string Part2(string input)
        {
            return Solve(input, 3);
        }

        private static string Solve(string input, int length)
        {
            return input
                .Trim()
                .Split("\n")
                .Subsets(length)
                .First(list => list.Sum(int.Parse) == 2020)
                .Aggregate(1, (acc, val) => acc * int.Parse(val))
                .ToString();
        }
    }
}