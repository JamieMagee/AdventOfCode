using System.Threading.Tasks;
using AdventOfCode._2015.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2015.Puzzles.Test.Solutions
{
    public sealed class Day01Test : TestBase<Day01>
    {
        [Fact]
        public async Task Part1()
        {
            Assert.Equal("0", await Solution.Part1Async("(())"));
            Assert.Equal("0", await Solution.Part1Async("()()"));
            Assert.Equal("3", await Solution.Part1Async("((("));
            Assert.Equal("3", await Solution.Part1Async("(()(()("));
            Assert.Equal("3", await Solution.Part1Async("))((((("));
            Assert.Equal("-1", await Solution.Part1Async("())"));
            Assert.Equal("-1", await Solution.Part1Async("))("));
            Assert.Equal("-3", await Solution.Part1Async(")))"));
            Assert.Equal("-3", await Solution.Part1Async(")())())"));

        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("1", await Solution.Part2Async(")"));
            Assert.Equal("5", await Solution.Part2Async("()())"));
        }
    }
}