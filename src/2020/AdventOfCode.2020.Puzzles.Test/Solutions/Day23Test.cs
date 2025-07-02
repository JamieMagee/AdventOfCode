using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day23Test : TestBase<Day23>
    {
        [Fact]
        public async Task Part1Async()
        {
            var input = """
            389125467
            """;
            Assert.Equal("67384529", await Solution.Part1Async(input));
        }

        [Fact]
        public async Task Part2Async()
        {
            var input = """
            389125467
            """;
            Assert.Equal("149245887792", await Solution.Part2Async(input));
        }
    }
}
