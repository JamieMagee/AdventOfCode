using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day02Test : TestBase<Day02>
    {
        private const string Input = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";

        [Fact]
        public async Task Part1()
        {
            Assert.Equal("2", await Solution.Part1Async(Input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("1", await Solution.Part2Async(Input));
        }
    }
}