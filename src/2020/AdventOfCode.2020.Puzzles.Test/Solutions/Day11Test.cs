namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day11Test : TestBase<Day11>
{
    private const string Input = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

    [Fact]
    public async Task Part1Async() => Assert.Equal("37", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("26", await this.Solution.Part2Async(Input));
}
