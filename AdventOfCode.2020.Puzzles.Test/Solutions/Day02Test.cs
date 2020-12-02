using System;
using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Core;
using AdventOfCode._2020.Puzzles.Solutions;
using AdventOfCode._2020.Puzzles.test;
using Xunit;

namespace AdventOfCode._2020.Puzzles.Test.Solutions
{
    
    public sealed class Day02Test : TestBase<Day02>
    {
        string input = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";
        
        [Fact]
        public async Task Part1()
        {
            
            Assert.Equal("2", await Solution.Part1Async(input));
        }

        [Fact]
        public async Task Part2()
        {
            Assert.Equal("1", await Solution.Part2Async(input));
        }
    }
}