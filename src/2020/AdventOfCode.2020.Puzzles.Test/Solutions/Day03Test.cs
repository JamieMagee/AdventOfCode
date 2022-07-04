namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day03Test : TestBase<Day03>
{
    private const string Input = @"..##.......
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
    public async Task Part1Async() => Assert.Equal("7", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("336", await this.Solution.Part2Async(Input));
}
