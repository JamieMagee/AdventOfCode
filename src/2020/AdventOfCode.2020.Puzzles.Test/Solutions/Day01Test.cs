namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public class Day01Test : TestBase<Day01>
{
    private const string Input = @"1721
979
366
299
675
1456";

    [Fact]
    public async Task Part1Async() => Assert.Equal("514579", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("241861950", await this.Solution.Part2Async(Input));
}
