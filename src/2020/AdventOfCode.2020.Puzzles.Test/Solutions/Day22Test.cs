using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day22Test : TestBase<Day22>
    {
        [Fact]
        public async Task Part1Async()
        {
            var input = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";
            Assert.Equal("306", await Solution.Part1Async(input));
        }

        [Fact]
        public async Task Part2Async()
        {
            var input = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";
            Assert.Equal("291", await Solution.Part2Async(input));
        }
    }
}
