using System;
using System.Linq;
using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Seating System")]
    public sealed class Day11 : SolutionBase
    {
        private const char Empty = 'L';
        private const char Occupied = '#';
        private const char Floor = '.';


        private static readonly (int y, int x)[] Kernel =
            {(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)};

        protected override string Part1(string input)
        {
            var parsedInput = input.GetLines<string>().ToArray();
            return Solve(parsedInput, 4, CountPart1).ToString();
        }


        protected override string Part2(string input)
        {
            var parsedInput = input.GetLines<string>().ToArray();
            return Solve(parsedInput, 5, CountPart2).ToString();
        }

        private static int Solve(string[] curr, int threshold,
            Func<string[], (int, int), int> countAdjacent)
        {
            string[] prev;
            do
            {
                prev = curr;
                curr = Step(curr, threshold, countAdjacent);
            } while (!prev.SequenceEqual(curr));

            return curr.SelectMany(r => r).Count(c => c == Occupied);
        }

        private static string[] Step(string[] grid, int threshold,
            Func<string[], (int, int), int> count)
        {
            var nextGrid = new string[grid.Length];
            for (var r = 0; r < grid.Length; r++)
            {
                var sb = new StringBuilder();
                for (var c = 0; c < grid[r].Length; c++)
                {
                    var adjacent = count(grid, (r, c));
                    switch (grid[r][c])
                    {
                        case Empty when adjacent == 0:
                            sb.Append(Occupied);
                            break;
                        case Occupied when adjacent >= threshold:
                            sb.Append(Empty);
                            break;
                        default:
                            sb.Append(grid[r][c]);
                            break;
                    }
                }

                nextGrid[r] = sb.ToString();
            }

            return nextGrid;
        }

        private static int CountPart1(string[] map, (int, int) coords)
        {
            return Kernel
                .Select(d => d.Add(coords))
                .Count(p => map.Get(p) == Occupied);
        }

        private static int CountPart2(string[] map, (int, int) coords)
        {
            return Kernel.Select(d =>
                    MoreEnumerable.Generate(coords,
                            p => d.Add(p))
                        .Skip(1)
                        .Select(map.Get)
                        .SkipWhile(c => c == Floor)
                        .First())
                .Count(x => x == Occupied);
        }
    }
}