using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day12Test : TestBase<Day12>
    {
        private const string Input = @"F10
N3
F7
R90
F11";
        
        [Fact]
        public async Task Part1()
        {
            
            Assert.Equal("25", await Solution.Part1Async(Input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("286", await Solution.Part2Async(Input));
        }
    }
}