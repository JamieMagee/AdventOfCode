using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day14Test : TestBase<Day14>
    {
        [Fact]
        public async Task Part1()
        {
            const string input = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";
            Assert.Equal("165", await Solution.Part1Async(input));
        }

        [Fact]
        public async Task Part2()
        {
            const string input = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";
            Assert.Equal("208", await Solution.Part2Async(input));
        }
    }
}