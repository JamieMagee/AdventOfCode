using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode.Core.Test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    public sealed class Day15Test : TestBase<Day15>
    {
        [Fact]
        public async Task Part1()
        {
            var testCases = new List<(string input, string expected)>
            {
                ("0,3,6", "436"),
                ("1,3,2", "1"),
                ("2,1,3", "10"),
                ("1,2,3", "27"),
                ("2,3,1", "78"),
                ("3,2,1", "438"),
                ("3,1,2", "1836")
            };
            foreach (var (input, expected) in testCases)
            {
                Assert.Equal(expected, await Solution.Part1Async(input));
            }
        }

        [Fact]
        public async Task Part2()
        {
            var testCases = new List<(string input, string expected)>
            {
                ("0,3,6", "175594"),
                ("1,3,2", "2578"),
                ("2,1,3", "3544142"),
                ("1,2,3", "261214"),
                ("2,3,1", "6895259"),
                ("3,2,1", "18"),
                ("3,1,2", "362")
            };
            foreach (var (input, expected) in testCases)
            {
                Assert.Equal(expected, await Solution.Part2Async(input));
            }
        }
    }
}