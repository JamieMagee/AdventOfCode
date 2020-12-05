using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public class Day01Test : TestBase<Day01>
    {
        private const string Input = @"1721
979
366
299
675
1456";

        [Fact]
        public async Task Part1()
        {
            Assert.Equal("514579", await Solution.Part1Async(Input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("241861950", await Solution.Part2Async(Input));
        }
    }
}