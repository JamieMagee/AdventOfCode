using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Core;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Password Philosophy")]
    public sealed class Day02 : SolutionBase
    {
        public override async Task<string> Part1Async(string input)
        {
            return input.Trim()
                .Split("\n")
                .Select(x => new PasswordPolicy(x))
                .Count(x => x.PartOneValid())
                .ToString();
        }

        public override async Task<string> Part2Async(string input)
        {
            return input.Trim()
                .Split("\n")
                .Select(x => new PasswordPolicy(x))
                .Count(x => x.PartTwoValid())
                .ToString();
        }

        private class PasswordPolicy
        {
            private readonly char _letter;
            private readonly int _max;
            private readonly int _min;
            private readonly string _password;

            public PasswordPolicy(string input)
            {
                var matches = Regex.Split(input, @"(\d+)\-(\d+) (\w): (\w+)");
                _min = int.Parse(matches[1]);
                _max = int.Parse(matches[2]);
                _letter = char.Parse(matches[3]);
                _password = matches[4];
            }

            public bool PartOneValid()
            {
                var count = _password.Count(x => x == _letter);
                return count >= _min && count <= _max;
            }

            public bool PartTwoValid()
            {
                return _password.Length >= _max - 1 &&
                       _password[_min - 1] == _letter ^ _password[_max - 1] == _letter;
            }
        }
    }
}