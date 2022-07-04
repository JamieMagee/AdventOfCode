namespace AdventOfCode._2015.Puzzles.Test.Solutions;

public sealed class Day01Test : TestBase<Day01>
{
    [Fact]
    public async Task Part1Async()
    {
        Assert.Equal("0", await this.Solution.Part1Async("(())"));
        Assert.Equal("0", await this.Solution.Part1Async("()()"));
        Assert.Equal("3", await this.Solution.Part1Async("((("));
        Assert.Equal("3", await this.Solution.Part1Async("(()(()("));
        Assert.Equal("3", await this.Solution.Part1Async("))((((("));
        Assert.Equal("-1", await this.Solution.Part1Async("())"));
        Assert.Equal("-1", await this.Solution.Part1Async("))("));
        Assert.Equal("-3", await this.Solution.Part1Async(")))"));
        Assert.Equal("-3", await this.Solution.Part1Async(")())())"));
    }

    [Fact]
    public async Task Part2Async()
    {
        Assert.Equal("1", await this.Solution.Part2Async(")"));
        Assert.Equal("5", await this.Solution.Part2Async("()())"));
    }
}
