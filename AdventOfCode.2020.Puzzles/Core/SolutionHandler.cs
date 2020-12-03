using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020.Puzzles.Core
{
    public class SolutionHandler : ISolutionHandler
    {
        public SolutionHandler()
        {
            Solutions = GatherPuzzleSolutions();
        }

        public IReadOnlyDictionary<int, SolutionMetadata> Solutions { get; }

        private static Dictionary<int, SolutionMetadata> GatherPuzzleSolutions()
        {
            var solutionsByDay = new Dictionary<int, SolutionMetadata>();
            var solutionInterface = typeof(ISolution);
            var solutionTypes = solutionInterface.Assembly.GetTypes()
                .Where(x => solutionInterface.IsAssignableFrom(x) && !x.IsAbstract)
                .ToList();

            foreach (var solutionType in solutionTypes)
            {
                var puzzleAttribute = solutionType.GetCustomAttributes(typeof(PuzzleAttribute), false)
                    .OfType<PuzzleAttribute>().FirstOrDefault();
                var day = puzzleAttribute?.Day ?? Convert.ToInt32(new Regex(@"[0-9]+").Match(solutionType.Name).Value);
                var title = puzzleAttribute?.Title ?? $"Puzzle #{day}";
                solutionsByDay.Add(day, new SolutionMetadata(solutionType, day, title));
            }

            return solutionsByDay;
        }
    }
}