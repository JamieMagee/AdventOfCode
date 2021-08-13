using System.Linq;
using AdventOfCode.Core;

namespace AdventOfCode._2015.Puzzles.Solutions
{
    [Puzzle("Not Quite Lisp")]
    public sealed class Day01 : SolutionBase
    {
        protected override string Part1(string input)
        {
            return input.Select(c => c == '(' ? 1 : -1)
                .Sum()
                .ToString();
        }

        protected override string Part2(string input)
        {
            var sum = 0;
            for (var i = 0; i < input.Length; i++)
            {
                sum += input[i] == '(' ? 1 : -1;
                if (sum < 0)
                {
                    return (i + 1).ToString();
                }
            }

            return string.Empty;
        }
    }
}