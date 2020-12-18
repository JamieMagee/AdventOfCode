using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day17Test : TestBase<Day17>
    {
        private const string Input = @".#.
..#
###";
        [Fact]
        public async Task Part1()
        {
            Assert.Equal("112", await Solution.Part1Async(Input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("848", await Solution.Part2Async(Input));
        }
    }
}