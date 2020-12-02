using System;
using AdventOfCode._2020.Puzzles.Core;

namespace AdventOfCode._2020.Puzzles.test
{
    public abstract class TestBase<TSolution> where TSolution : ISolution
    {
        protected TSolution Solution { get; private set; }


        protected TestBase()
        {
            Solution = Activator.CreateInstance<TSolution>();
        }
    }
}