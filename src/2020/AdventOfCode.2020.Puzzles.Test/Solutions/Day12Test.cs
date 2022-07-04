namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day12Test : TestBase<Day12>
{
    private const string Input = @"F10
N3
F7
R90
F11";

    [Fact]
    public async Task Part1Async() => Assert.Equal("25", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("286", await this.Solution.Part2Async(Input));
}
