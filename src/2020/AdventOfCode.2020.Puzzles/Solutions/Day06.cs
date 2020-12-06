using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Custom Customs")]
    public sealed class Day06 : SolutionBase
    {
        protected override string Part1(string input)
        {
            return ParseInput(input).Select(x => new string(
                        x.Replace("\n", "")
                            .Distinct()
                            .ToArray()
                    )
                )
                .Sum(x => x.Length)
                .ToString();
        }

        protected override string Part2(string input)
        {
            return ParseInput(input).Select(x =>
                    x.Split("\n").Aggregate((a, b) => new string(a.Intersect(b).ToArray()))
                )
                .Sum(x => x.Length)
                .ToString();
        }

        private static IEnumerable<string> ParseInput(string input)
        {
            return input.Trim().Split("\n\n");
        }
    }
}