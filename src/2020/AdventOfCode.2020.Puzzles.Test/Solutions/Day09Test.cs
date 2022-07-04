namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day09Test : TestBase<Day09>
{
    private const string Input = @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";

    [Fact]
    public async Task Part1Async()
    {
        this.Solution.PreambleLength = 5;
        Assert.Equal("127", await this.Solution.Part1Async(Input));
    }

    [Fact]
    public async Task Part2Async()
    {
        this.Solution.PreambleLength = 5;
        Assert.Equal("62", await this.Solution.Part2Async(Input));
    }
}
