using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Adapter Array")]
    public sealed class Day10 : SolutionBase
    {
        protected override string Part1(string input)
        {
            var parsedInput = input.GetLines<int>().ToList();
            parsedInput.Add(0);
            parsedInput.Add(parsedInput.Max() + 3);
            var differences = parsedInput
                .OrderBy(x => x)
                .Pairwise((a, b) => b - a)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count());
            return (differences[1] * differences[3]).ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = input.GetLines<int>().ToList();
            parsedInput.Add(0);
            parsedInput.Add(parsedInput.Max() + 3);
            parsedInput.Sort();
            var memoize = new Dictionary<int, long>
            {
                [parsedInput.Count - 1] = 1
            };
            for (var k = parsedInput.Count - 2; k >= 0; k--)
            {
                var connections = 0L;
                for (var connected = k + 1;
                    connected < parsedInput.Count && parsedInput[connected] - parsedInput[k] <= 3;
                    connected++)
                {
                    connections += memoize[connected];
                }

                memoize[k] = connections;
            }

            return memoize[0].ToString();
        }
    }
}