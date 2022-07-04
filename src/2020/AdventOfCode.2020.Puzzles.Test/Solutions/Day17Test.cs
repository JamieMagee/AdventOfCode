namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day17Test : TestBase<Day17>
{
    private const string Input = @".#.
..#
###";

    [Fact]
    public async Task Part1Async() => Assert.Equal("112", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("848", await this.Solution.Part2Async(Input));
}
