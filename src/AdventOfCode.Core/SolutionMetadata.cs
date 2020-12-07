using System;

namespace AdventOfCode.Core
{
    public sealed class SolutionMetadata
    {
        public SolutionMetadata(Type type, int year, int day, string name)
        {
            Type = type;
            Year = year;
            Day = day;
            Title = name;
        }
        
        public int Year { get;  }

        public int Day { get; }

        public Type Type { get; }

        public string Title { get; }

        public ISolution CreateInstance()
        {
            return (ISolution) Activator.CreateInstance(Type);
        }
    }
}