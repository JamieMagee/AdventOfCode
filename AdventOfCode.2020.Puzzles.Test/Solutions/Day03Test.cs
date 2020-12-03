using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode._2020.Puzzles.test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day03Test : TestBase<Day03>
    {
        private string input = @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
        
        [Fact]
        public async Task Part1()
        {
            Assert.Equal("7", await Solution.Part1Async(input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("336", await Solution.Part2Async(input));
        }
    }
}