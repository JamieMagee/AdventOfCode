using System;

namespace AdventOfCode.Core.Test
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