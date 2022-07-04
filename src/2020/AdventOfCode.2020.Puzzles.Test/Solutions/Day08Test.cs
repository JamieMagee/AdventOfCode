namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day08Test : TestBase<Day08>
{
    private const string Input = @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

    [Fact]
    public async Task Part1Async() => Assert.Equal("5", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("8", await this.Solution.Part2Async(Input));
}
