namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day05Test : TestBase<Day05>
{
    [Fact]
    public async Task Part1Async()
    {
        const string input1 = "BFFFBBFRRR";
        Assert.Equal("567", await this.Solution.Part1Async(input1));
        const string input2 = "FFFBBBFRRR";
        Assert.Equal("119", await this.Solution.Part1Async(input2));
        const string input3 = "BBFFBBFRLL";
        Assert.Equal("820", await this.Solution.Part1Async(input3));
    }
}
