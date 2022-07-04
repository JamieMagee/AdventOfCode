namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day10Test : TestBase<Day10>
{
    private const string Input = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";

    [Fact]
    public async Task Part1Async() => Assert.Equal("220", await this.Solution.Part1Async(Input));

    [Fact]
    public async Task Part2Async() => Assert.Equal("19208", await this.Solution.Part2Async(Input));
}
