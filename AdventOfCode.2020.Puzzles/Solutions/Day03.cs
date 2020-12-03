using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Toboggan Trajectory")]
    public sealed class Day03 : SolutionBase
    {
        private const char TREE = '#';
        private const int DAY_1_X = 3;
        private const int DAY_1_Y = 1;

        private readonly List<(int, int)> Day2Slopes = new List<(int, int)>
        {
            (1, 1),
            (3, 1),
            (5, 1),
            (7, 1),
            (1, 2)
        };

        public override string Part1(string input)
        {
            var trimmed = input.Trim()
                .Split('\n')
                .Select(line => line.ToCharArray())
                .ToList();
            int x = 0, y = 0, countTrees = 0;
            do
            {
                x = (x + DAY_1_X) % trimmed[0].Length;
                y++;
                var item = trimmed[y][x];
                countTrees += item == TREE ? 1 : 0;
            } while (y < trimmed.Count - 1);

            return countTrees.ToString();
        }

        public override string Part2(string input)
        {
            return Day2Slopes.Select(tuple => CalculateTrees(input, tuple.Item1, tuple.Item2))
                .Aggregate((uint)1, (acc, val) => acc * val)
                .ToString();
        }

        private uint CalculateTrees(string input, int xIncrement, int yIncrement)
        {
            var trimmed = input.Trim()
                .Split('\n')
                .Select(line => line.ToCharArray())
                .ToList();
            int x = 0, y = 0;
            uint countTrees = 0;
            do
            {
                x = (x + xIncrement) % trimmed[0].Length;
                y += yIncrement;
                var item = trimmed[y][x];
                countTrees += item == TREE ? 1 : 0;
            } while (y < trimmed.Count - 1);

            return countTrees;
        }
    }
}