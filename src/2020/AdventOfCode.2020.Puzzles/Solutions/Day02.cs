namespace AdventOfCode._2020.Puzzles.Solutions;

using System.Globalization;
using System.Text.RegularExpressions;

[Puzzle("Password Philosophy")]
public sealed class Day02 : SolutionBase
{
    protected override string Part1(string input) => input.Trim()
        .Split("\n")
        .Select(x => new PasswordPolicy(x))
        .Count(x => x.PartOneValid())
        .ToString(CultureInfo.InvariantCulture);

    protected override string Part2(string input) => input.Trim()
        .Split("\n")
        .Select(x => new PasswordPolicy(x))
        .Count(x => x.PartTwoValid())
        .ToString(CultureInfo.InvariantCulture);

    private sealed record PasswordPolicy
    {
        private readonly char letter;
        private readonly int max;
        private readonly int min;
        private readonly string password;

        public PasswordPolicy(string input)
        {
            var matches = Regex.Split(input, @"(\d+)\-(\d+) (\w): (\w+)");
            this.min = int.Parse(matches[1], CultureInfo.InvariantCulture);
            this.max = int.Parse(matches[2], CultureInfo.InvariantCulture);
            this.letter = char.Parse(matches[3]);
            this.password = matches[4];
        }

        public bool PartOneValid()
        {
            var count = this.password.Count(x => x == this.letter);
            return count >= this.min && count <= this.max;
        }

        public bool PartTwoValid() => (this.password[this.min - 1] == this.letter) ^ (this.password[this.max - 1] == this.letter);
    }
}
