using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using Xunit;

namespace AdventOfCode._2020.Puzzles.test.Solutions
{
    public class Day01Test : TestBase<Day01>
    {
        string input = @"1721
979
366
299
675
1456";
        
        [Fact]
        public async Task Part1()
        {
            Assert.Equal("514579", await Solution.Part1Async(input));
        }
        
        [Fact]
        public async Task Part2()
        {
            Assert.Equal("241861950", await Solution.Part2Async(input));
        }
    }
}