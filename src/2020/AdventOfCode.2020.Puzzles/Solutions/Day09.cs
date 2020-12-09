using System.Linq;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Encoding Error")]
    public sealed class Day09 : SolutionBase
    {
        public int PreambleLength { get; set; } = 25;
        
        protected override string Part1(string input)
        {
            var parsedInput = input.GetLines<long>().ToArray();
            return parsedInput.Skip(PreambleLength)
                .Where((x, i) => parsedInput[i..(i + PreambleLength)].Subsets(2).All(l => l.Sum() != x))
                .First()
                .ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = input.GetLines<long>().ToArray();
            var invalid = long.Parse(Part1(input));
            for (var i = 0; i < parsedInput.Length; i++)
            {
                var sum = parsedInput[i];
                for (var j = i + 1; j < parsedInput.Length && sum < invalid; j++)
                {
                    sum += parsedInput[j];
                    if (sum > invalid)
                    {
                        break;
                    }
                    if (sum == invalid)
                    {
                        var range = parsedInput[i..j];
                        return (range.Min() + range.Max()).ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}