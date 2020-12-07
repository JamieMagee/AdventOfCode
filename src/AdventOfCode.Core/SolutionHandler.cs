using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core
{
    public class SolutionHandler : ISolutionHandler
    {
        public SolutionHandler()
        {
            Solutions = GatherPuzzleSolutions();
        }

        public IDictionary<int, IDictionary<int, SolutionMetadata>> Solutions { get; }

        private static IDictionary<int, IDictionary<int, SolutionMetadata>> GatherPuzzleSolutions()
        {
            var solutionsByYear = new Dictionary<int, IDictionary<int, SolutionMetadata>>();
            
            AssemblyLoader.LoadAssemblies();
            var solutionTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ISolution).IsAssignableFrom(x) && !x.IsAbstract);
            var years = solutionTypes.Select(t => Convert.ToInt32(new Regex(@"20[0-9]+").Match(t.FullName).Value))
                .Distinct();

            foreach (var year in years)
            {
                var solutionsByDay = new Dictionary<int, SolutionMetadata>();
                foreach (var solutionType in solutionTypes.Where(t => t.FullName.Contains(year.ToString())))
                {
                    var puzzleAttribute = solutionType.GetCustomAttributes(typeof(PuzzleAttribute), false)
                        .OfType<PuzzleAttribute>().FirstOrDefault();
                    var day = puzzleAttribute?.Day ?? Convert.ToInt32(new Regex(@"[0-9]+").Match(solutionType.Name).Value);
                    var title = puzzleAttribute?.Title ?? $"Puzzle #{day}";
                    solutionsByDay.Add(day, new SolutionMetadata(solutionType, year, day, title));
                }
                solutionsByYear.Add(year, solutionsByDay);
            }

            return solutionsByYear;
        }
    }
}