namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day13Test : TestBase<Day13>
{
    private const string Input = @"939
7,13,x,x,59,x,31,19";

    [Fact]
    public async Task Part1Async() => Assert.Equal("295", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("1068781", await this.Solution.Part2Async(Input));
}
