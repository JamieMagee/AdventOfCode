﻿namespace AdventOfCode._2020.Puzzles.Test.Solutions;

public sealed class Day18Test : TestBase<Day18>
{
    [Fact]
    public async Task Part1Async()
    {
        var testCases = new List<(string Input, string Expected)>
        {
            ("1 + 2 * 3 + 4 * 5 + 6", "71"),
            ("1 + (2 * 3) + (4 * (5 + 6))", "51"),
            ("2 * 3 + (4 * 5)", "26"),
            ("5 + (8 * 3 + 9 + 3 * 4 * 3)", "437"),
            ("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", "12240"),
            ("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", "13632"),
        };
        foreach (var (input, expected) in testCases)
        {
            Assert.Equal(expected, await this.Solution.Part1Async(input));
        }
    }

    [Fact]
    public async Task Part2Async()
    {
        var testCases = new List<(string Input, string Expected)>
        {
            ("1 + 2 * 3 + 4 * 5 + 6", "231"),
            ("1 + (2 * 3) + (4 * (5 + 6))", "51"),
            ("2 * 3 + (4 * 5)", "46"),
            ("5 + (8 * 3 + 9 + 3 * 4 * 3)", "1445"),
            ("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", "669060"),
            ("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", "23340"),
        };
        foreach (var (input, expected) in testCases)
        {
            Assert.Equal(expected, await this.Solution.Part2Async(input));
        }
    }
}
