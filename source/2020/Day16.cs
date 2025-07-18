namespace AdventOfCode._2020.Puzzles.Solutions;

using System.Globalization;
using System.Text.RegularExpressions;
using MoreLinq;

[Puzzle("Ticket Translation")]
public sealed class Day16 : SolutionBase
{
    private readonly Regex rulesRegex = new(@"(?<name>(\w+\s?)+):( (?<lower>\d+)\-(?<upper>\d+)(?: or)?){2}");
    private readonly Regex ticketRegex = new(@"^((?:\d,?)+)$", RegexOptions.Multiline);

    public Func<string, bool> FieldFilter { get; set; } = s => s.StartsWith("departure", StringComparison.Ordinal);

    protected override string Part1(string input)
    {
        var ticketMatches = this.ticketRegex.Matches(input);
        var nearbyTickets = ticketMatches.ToArray()[1..].Select(l => l.Value.Split(',').Select(int.Parse));

        var rulesMatches = this.rulesRegex.Matches(input);
        var rules = rulesMatches.Select(m => (name: m.Groups["name"].Value, validators:
            m.Groups["lower"].Captures.EquiZip<Capture, Capture, Func<int, bool>>(
                m.Groups["upper"].Captures,
                (l, u) => n => n >= int.Parse(l.Value, CultureInfo.InvariantCulture) && n <= int.Parse(u.Value, CultureInfo.InvariantCulture))));

        return nearbyTickets.Aggregate(
                0,
                (current, nearbyTicket) => current + nearbyTicket
                    .Where(n => !rules.Any(r => r.validators.Any(s => s(n))))
                    .Aggregate(0, (x, y) => x + y))
            .ToString(CultureInfo.InvariantCulture);
    }

    public override async Task<string> Part2Async(string input)
    {
        var ticketMatches = this.ticketRegex.Matches(input);
        var myTicket = ticketMatches[0].Value.Split(',').Select(int.Parse).ToArray();
        var nearbyTickets = ticketMatches.ToArray()[1..]
            .Select(l => l.Value.Split(',')
                .Select(int.Parse)
                .ToArray());

        var rulesMatches = this.rulesRegex.Matches(input);
        var rules = rulesMatches.Select(m => (
                name: m.Groups["name"].Value,
                validators: m.Groups["lower"].Captures.EquiZip<Capture, Capture, Func<int, bool>>(
                    m.Groups["upper"].Captures,
                    (l, u) => n => n >= int.Parse(l.Value, CultureInfo.InvariantCulture) && n <= int.Parse(u.Value, CultureInfo.InvariantCulture))))
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

        var map = new List<(string Name, int Index)>();
        while (map.Count < rules.Length)
        {
            if (this.IsUpdateProgressNeeded())
            {
                await this.UpdateProgressAsync(map.Count, rules.Length);
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
            .Where(r => this.FieldFilter(r.Name))
            .Select(r => myTicket[r.Index])
            .Aggregate(1L, (l, r) => l * r)
            .ToString(CultureInfo.InvariantCulture);
    }
}
