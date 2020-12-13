using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day13Test : TestBase<Day13>
    {
        private const string Input = @"939
7,13,x,x,59,x,31,19";

        [Fact]
        public async Task Part1()
        {
            Assert.Equal("295", await Solution.Part1Async(Input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("1068781", await Solution.Part2Async(Input));
        }
    }
}