using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Report Repair")]
    public sealed class Day01 : SolutionBase
    {
        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input).ToList();
            return parsedInput.Where(x => parsedInput.Contains(2020 - x))
                .Aggregate(1, (a, b) => a * b)
                .ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input).ToList();
            return parsedInput.Where(x => 0 < parsedInput.FirstOrDefault(y => parsedInput.Contains(2020 - x - y)))
                .Aggregate(1, (a, b) => a * b).ToString();
        }

        private static IEnumerable<int> ParseInput(string input)
        {
            return GetLines(input).Select(int.Parse);
        }
    }
}