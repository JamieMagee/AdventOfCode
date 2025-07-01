namespace AdventOfCode._2020.Puzzles.Solutions;

using System.Numerics;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using MoreLinq.Extensions;

[Puzzle("Shuttle Search")]
public sealed class Day13 : SolutionBase
{
    protected override string Part1(string input)
    {
        var parsedInput = input.GetLines<string>().ToList();
        var start = int.Parse(parsedInput[0]);
        return parsedInput[1].Split(',')
            .Where(x => x != "x")
            .Select(int.Parse)
            .Select(x => (x - (start % x), x))
            .Minima(x => x.Item1)
            .Select(x => x.Item1 * x.x)
            .First()
            .ToString();
    }

    protected override string Part2(string input) => ChineseRemainderTheorem(
        input.GetLines<string>()
            .ToList()[1]
            .Split(',')
            .Select((bus, i) => (bus, i))
            .Where(tuple => tuple.bus != "x")
            .Select(tuple => (mod: long.Parse(tuple.bus), a: long.Parse(tuple.bus) - tuple.i)))
    .ToString();

    private static long ChineseRemainderTheorem(IEnumerable<(long Mod, long A)> tuples)
    {
        var product = tuples.Aggregate(1L, (acc, tuple) => acc * tuple.Mod);
        var sum = tuples.Select((item, i) =>
            {
                var p = product / item.Mod;
                return item.A * (long)BigInteger.ModPow(p, item.Mod - 2, item.Mod) * p;
            })
            .Sum();

        return sum % product;
    }
}
