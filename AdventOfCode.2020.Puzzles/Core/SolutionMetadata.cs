using System;

namespace AdventOfCode._2020.Puzzles.Core
{
    public sealed class SolutionMetadata
    {
        public int Day { get; }

        public Type Type { get; }

        public string Title { get; }

        public SolutionMetadata(Type type, int day, string name)
        {
            Type = type;
            Day = day;
            Title = name;
        }

        public ISolution CreateInstance()
        {
            return (ISolution)Activator.CreateInstance(Type);
        }
    }
}