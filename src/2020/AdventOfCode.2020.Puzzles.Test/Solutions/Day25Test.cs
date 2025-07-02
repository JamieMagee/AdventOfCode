using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day25Test : TestBase<Day25>
    {
        [Fact]
        public async Task Part1Async()
        {
            var input = """
            5764801
            17807724
            """;
            Assert.Equal("14897079", await Solution.Part1Async(input));
        }
    }
}
