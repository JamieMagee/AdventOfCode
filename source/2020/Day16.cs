using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Ticket Translation")]
    public sealed class Day16 : SolutionBase
    {
        private readonly Regex _rulesRegex = new(@"(?<name>(\w+\s?)+):( (?<lower>\d+)\-(?<upper>\d+)(?: or)?){2}");
        private readonly Regex _ticketRegex = new(@"^((?:\d,?)+)$", RegexOptions.Multiline);
        public Func<string, bool> FieldFilter = s => s.StartsWith("departure"); 

        protected override string Part1(string input)
        {
            var ticketMatches = _ticketRegex.Matches(input);
            var nearbyTickets = ticketMatches.ToArray()[1..].Select(l => l.Value.Split(',').Select(int.Parse));

            var rulesMatches = _rulesRegex.Matches(input);
            var rules = rulesMatches.Select(m => (name: m.Groups["name"].Value, validators:
                m.Groups["lower"].Captures.EquiZip<Capture, Capture, Func<int, bool>>(m.Groups["upper"].Captures,
                    (l, u) => n => n >= int.Parse(l.Value) && n <= int.Parse(u.Value))));

            return nearbyTickets.Aggregate(0,
                    (current, nearbyTicket) => current + nearbyTicket
                        .Where(n => !rules.Any(r => r.validators.Any(s => s(n))))
                        .Aggregate(0, (x, y) => x + y))
                .ToString();
        }

        public override async Task<string> Part2Async(string input)
        {
            var ticketMatches = _ticketRegex.Matches(input);
            var myTicket = ticketMatches[0].Value.Split(',').Select(int.Parse).ToArray();
            var nearbyTickets = ticketMatches.ToArray()[1..]
                .Select(l => l.Value.Split(',')
                    .Select(int.Parse)
                    .ToArray()
                );

            var rulesMatches = _rulesRegex.Matches(input);
            var rules = rulesMatches.Select(m => (
                    name: m.Groups["name"].Value,
                    validators: m.Groups["lower"].Captures.EquiZip<Capture, Capture, Func<int, bool>>(
                        m.Groups["upper"].Captures,
                        (l, u) => n => n >= int.Parse(l.Value) && n <= int.Parse(u.Value))))
                .ToArray();

            var validTickets = nearbyTickets.Where(nearbyTicket =>
                    nearbyTicket.All(n => rules.Any(r => r.validators.Any(s => s(n)))))
                .ToArray();

            var candidates = rules.Select(r =>
                    (r.name,
                        indexes: Enumerable.Range(0, validTickets[0].Length)
                            .Where(i => validTickets.All(t => r.validators.Any(v => v(t[i]))))
                            .ToList()))
                .ToList();

            var map = new List<(string name, int index)>();
            while (map.Count < rules.Length)
            {
                if (IsUpdateProgressNeeded())
                {
                    await UpdateProgressAsync(map.Count, rules.Length);
                }

                var (name, ints) = candidates.First(c => c.indexes.Count == 1);
                map.Add((name, ints[0]));
                candidates.RemoveAll(c => c.name == name);
                foreach (var (_, indexes) in candidates)
                {
                    indexes.Remove(ints[0]);
                }
            }

            return map
                .Where(r => FieldFilter(r.name))
                .Select(r => myTicket[r.index])
                .Aggregate(1L, (l, r) => l * r)
                .ToString();
        }
    }
}