namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day16Test : TestBase<Day16>
{
    [Fact]
    public async Task Part1Async()
    {
        const string input = @"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";
        Assert.Equal("71", await this.Solution.Part1Async(input));
    }

    [Fact]
    public async Task Part2Async()
    {
        const string input = @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9";
        this.Solution.FieldFilter = s => s is "row" or "seat";
        Assert.Equal("143", await this.Solution.Part2Async(input));
    }
}
