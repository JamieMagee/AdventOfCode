namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day14Test : TestBase<Day14>
{
    [Fact]
    public async Task Part1Async()
    {
        const string input = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";
        Assert.Equal("165", await this.Solution.Part1Async(input));
    }

    [Fact]
    public async Task Part2Async()
    {
        const string input = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";
        Assert.Equal("208", await this.Solution.Part2Async(input));
    }
}
