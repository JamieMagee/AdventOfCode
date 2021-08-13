using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Passport Processing")]
    public sealed class Day04 : SolutionBase
    {
        private static readonly IDictionary<string, string> PassportFields = new Dictionary<string, string>
        {
            {"byr", @"^(19[2-8][0-9]|199[0-9]|200[0-2])$"},
            {"iyr", @"^(201[0-9]|2020)$"},
            {"eyr", @"^(202[0-9]|2030)$"},
            {"hgt", @"^(((1[5-8][0-9]|19[0-3])cm)|((59|6[0-9]|7[0-6])in))$"},
            {"hcl", @"^#[0-9a-fA-F]{6}$"},
            {"ecl", @"^(amb|blu|brn|gry|grn|hzl|oth)$"},
            {"pid", @"^\d{9}$"}
        };

        protected override string Part1(string input)
        {
            return ParseInput(input)
                .Count(p =>
                    PassportFields.All(f => p.Select(e => e.Key).Contains(f.Key))
                )
                .ToString();
        }

        protected override string Part2(string input)
        {
            return ParseInput(input)
                .Count(p =>
                    PassportFields.All(f => p.Select(e => e.Key).Contains(f.Key)) &&
                    PassportFields.All(f => Regex.IsMatch(p.First(e => e.Key == f.Key).Value, f.Value))
                )
                .ToString();
        }

        private static IEnumerable<IDictionary<string, string>> ParseInput(string input)
        {
            return input.Trim()
                .Split("\n\n")
                .Select(passport =>
                    passport.Split(' ', '\n')
                        .Select(field =>
                            {
                                var tuple = field.Split(':');
                                return (tuple[0], tuple[1]);
                            }
                        )
                        .ToDictionary(e => e.Item1, e => e.Item2)
                );
        }
    }
}