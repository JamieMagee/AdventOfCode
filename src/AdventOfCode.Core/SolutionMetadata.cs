using System;

namespace AdventOfCode.Core
{
    public sealed class SolutionMetadata
    {
        public SolutionMetadata(Type type, int day, string name)
        {
            Type = type;
            Day = day;
            Title = name;
        }

        public int Day { get; }

        public Type Type { get; }

        public string Title { get; }

        public ISolution CreateInstance()
        {
            return (ISolution) Activator.CreateInstance(Type);
        }
    }
}