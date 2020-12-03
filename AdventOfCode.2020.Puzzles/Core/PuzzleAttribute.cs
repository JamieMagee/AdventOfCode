using System;

namespace AdventOfCode._2020.Puzzles.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class PuzzleAttribute : Attribute
    {
        public PuzzleAttribute(string title = null, int day = -1)
        {
            Day = day < 0 ? null : (int?) day;
            Title = title;
        }

        public int? Day { get; }

        public string Title { get; }
    }
}