using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Conway Cubes")]
    public sealed class Day17 : SolutionBase
    {
        private static readonly IEnumerable<(int x, int y, int z)> Kernel3D = Enumerable.Range(-1, 3)
            .SelectMany(x => Enumerable.Range(-1, 3)
                .SelectMany(y => Enumerable.Range(-1, 3)
                    .Select(z => (x, y, z))))
            .Where(coords => coords != (0, 0, 0));

        private static readonly IEnumerable<(int x, int y, int z, int w)> Kernel4D = Enumerable.Range(-1, 3)
            .SelectMany(x => Enumerable.Range(-1, 3)
                .SelectMany(y => Enumerable.Range(-1, 3)
                    .SelectMany(z => Enumerable.Range(-1, 3)
                        .Select(w => (x, y, z, w)))))
            .Where(coords => coords != (0, 0, 0, 0));

        protected override string Part1(string input)
        {
            var state = new Dictionary<(int x, int y, int z), bool>();
            var lines = input.GetLines<string>()
                .ToList();

            for (var x = 0; x < lines.Count; x++)
            {
                for (var y = 0; y < lines[x].Length; y++)
                {
                    state[(x, y, 0)] = lines[x][y] == '#';
                }
            }

            var count = new Dictionary<(int x, int y, int z), int>();
            for (var i = 0; i < 6; i++)
            {
                count.Clear();
                state.Keys.ForEach(k => count[k] = 0);

                foreach (var ((x, y, z), alive) in state.Where(kvp => kvp.Value))
                foreach (var (dx, dy, dz) in Kernel3D)
                {
                    count[(x + dx, y + dy, z + dz)] =
                        count.GetValueOrDefault((x + dx, y + dy, z + dz)) + 1;
                }

                foreach (var (p, c) in count)
                {
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false
                    };
                }
            }

            return state.Values.Count(x => x).ToString();
        }

        protected override string Part2(string input)
        {
            var state = new Dictionary<(int x, int y, int z, int w), bool>();
            var lines = input.GetLines<string>()
                .ToList();

            for (var x = 0; x < lines.Count; x++)
            {
                for (var y = 0; y < lines[x].Length; y++)
                {
                    state[(x, y, 0, 0)] = lines[x][y] == '#';
                }
            }

            var count = new Dictionary<(int x, int y, int z, int w), int>();
            for (var i = 0; i < 6; i++)
            {
                count.Clear();
                state.Keys.ForEach(k => count[k] = 0);

                foreach (var ((x, y, z, w), alive) in state.Where(kvp => kvp.Value))
                foreach (var (dx, dy, dz, dw) in Kernel4D)
                {
                    count[(x + dx, y + dy, z + dz, w + dw)] =
                        count.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;
                }

                foreach (var (p, c) in count)
                {
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false
                    };
                }
            }

            return state.Values.Count(x => x).ToString();
        }
    }
}