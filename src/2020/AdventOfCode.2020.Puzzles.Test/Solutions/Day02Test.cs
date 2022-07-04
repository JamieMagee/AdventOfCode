namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day02Test : TestBase<Day02>
{
    private const string Input = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";

    [Fact]
    public async Task Part1Async() => Assert.Equal("2", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("1", await this.Solution.Part2Async(Input));
}
