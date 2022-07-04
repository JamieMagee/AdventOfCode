namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day06Test : TestBase<Day06>
{
    private const string Input = @"abc

a
b
c

ab
ac

a
a
a
a

b";

    [Fact]
    public async Task Part1Async() => Assert.Equal("11", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("6", await this.Solution.Part2Async(Input));
}
