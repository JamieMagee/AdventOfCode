using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day05Test : TestBase<Day05>
    {
        [Fact]
        public async Task Part1()
        {
            const string input1 = "BFFFBBFRRR";
            Assert.Equal("567", await Solution.Part1Async(input1));
            const string input2 = "FFFBBBFRRR";
            Assert.Equal("119", await Solution.Part1Async(input2));
            const string input3 = "BBFFBBFRLL";
            Assert.Equal("820", await Solution.Part1Async(input3));
        }
    }
}