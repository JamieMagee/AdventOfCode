using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Rambunctious Recitation")]
    public sealed class Day15 : SolutionBase
    {
        public override async Task<string> Part1Async(string input)
        {
            var numbers = input.Split(',')
                .Select(int.Parse)
                .ToArray();

            return await Solve(numbers, 2020);
        }

        public override async Task<string> Part2Async(string input)
        {
            var numbers = input.Split(',')
                .Select(int.Parse)
                .ToArray();
            return await Solve(numbers, 30_000_000);
        }

        private async Task<string> Solve(IReadOnlyList<int> input, int number)
        {
            var spoken = new int[number];
            Array.Fill(spoken, -1);

            var turn = 1;
            for (; turn < input.Count + 1; turn++)
            {
                spoken[input[turn - 1]] = turn;
            }

            var currentNumber = 0;
            for (; turn < number; turn++)
            {
                if (IsUpdateProgressNeeded())
                {
                    await UpdateProgressAsync(turn, number);
                }

                var prevTime = spoken[currentNumber];
                spoken[currentNumber] = turn;
                currentNumber = prevTime != -1 ? turn - prevTime : 0;
            }

            return currentNumber.ToString();
        }
    }
}