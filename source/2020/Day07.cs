using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Handy Haversacks")]
    public sealed class Day07 : SolutionBase
    {
        private const string Colour = "shiny gold";

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            bool Contains(string colour) =>
                parsedInput[colour].ContainsKey(Colour) || parsedInput[colour].Keys.Any(Contains);
            return parsedInput.Keys.Count(Contains).ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            int MustContain(string colour) => parsedInput[colour].Sum(kvp => kvp.Value * (1 + MustContain(kvp.Key)));
            return MustContain(Colour).ToString();
        }

        private static Dictionary<string, Dictionary<string, int>> ParseInput(string input)
        {
            return GetLines(input).Select(x => Regex.Match(x, @"(.*) bags contain(?: (\d+ .*?) bags?[,.])*"))
                .ToDictionary(
                    x => x.Groups[1].Value,
                    x => x.Groups[2].Captures
                        .Select(y => y.Value.Split(' ', 2))
                        .ToDictionary(
                            z => z[1],
                            z => int.Parse(z[0])
                        )
                );
        }
    }
}