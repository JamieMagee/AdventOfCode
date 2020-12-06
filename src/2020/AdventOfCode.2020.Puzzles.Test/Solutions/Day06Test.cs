using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day06Test : TestBase<Day06>
    {
        private const string Input = @"abc

a
b
c

ab
ac

a
a
a
a

b";
        
        [Fact]
        public async Task Part1()
        {
            
            Assert.Equal("11", await Solution.Part1Async(Input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("6", await Solution.Part2Async(Input));
        }
    }
}