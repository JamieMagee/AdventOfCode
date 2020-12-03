using System;
using AdventOfCode._2020.Puzzles.Core;

namespace AdventOfCode._2020.Puzzles.Test
{
    public abstract class TestBase<TSolution> where TSolution : ISolution
    {
        protected TestBase()
        {
            Solution = Activator.CreateInstance<TSolution>();
        }

        protected TSolution Solution { get; }
    }
}