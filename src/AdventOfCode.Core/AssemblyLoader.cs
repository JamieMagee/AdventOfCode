using System;
using System.Linq;

namespace AdventOfCode.Core
{
    public static class AssemblyLoader
    {
        private const int Start = 2015;
        private static readonly int Current = DateTime.Now.Month == 12 ? DateTime.Now.Year : DateTime.Now.Year - 1;

        public static void LoadAssemblies()
        {
            Enumerable.Range(Start, Current - Start + 1)
                .ToList()
                .ForEach(year =>
                {
                    try
                    {
                        Activator.CreateInstance($"AdventOfCode.{year}.Puzzles",
                            $"AdventOfCode._{year}.Puzzles.Solutions.Day01");
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                });
        }
    }
}