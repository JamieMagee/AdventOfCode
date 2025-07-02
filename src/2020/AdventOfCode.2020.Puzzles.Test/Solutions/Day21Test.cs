using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day21Test : TestBase<Day21>
    {
        [Fact]
        public async Task Part1Async()
        {
            var input = """
            mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
            trh fvjkl sbzzf mxmxvkd (contains dairy)
            sqjhc fvjkl (contains soy)
            sqjhc mxmxvkd sbzzf (contains fish)
            """;
            Assert.Equal("5", await Solution.Part1Async(input));
        }

        [Fact]
        public async Task Part2Async()
        {
            var input = """
            mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
            trh fvjkl sbzzf mxmxvkd (contains dairy)
            sqjhc fvjkl (contains soy)
            sqjhc mxmxvkd sbzzf (contains fish)
            """;
            Assert.Equal("mxmxvkd,sqjhc,fvjkl", await Solution.Part2Async(input));
        }
    }
}
