using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;
using MoreLinq;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Docking Data")]
    public sealed class Day14 : SolutionBase
    {
        private static readonly Regex Regex =
            new(@"^(mask = (?<mask>[01X]{36}))|(mem\[(?<address>\d+)\] = (?<value>\d+))$");

        protected override string Part1(string input)
        {
            var matches = input.GetLines<string>()
                .Select(l => Regex.Match(l))
                .ToArray();

            var memory = new Dictionary<int, ulong>();
            var mask = string.Empty;
            foreach (var match in matches)
            {
                if (match.Groups["mask"].Success)
                {
                    mask = match.Groups["mask"].Value;
                }
                else
                {
                    var value = Convert.ToString(Convert.ToInt32(match.Groups["value"].Value), 2)
                        .PadLeft(36, '0');
                    var result =
                        Convert.ToUInt64(new string(value.Select((c, i) => mask[i] == 'X' ? c : mask[i]).ToArray()), 2);
                    var address = int.Parse(match.Groups["address"].Value);
                    memory[address] = result;
                }
            }

            return memory.Values.Aggregate((a, c) => a + c).ToString();
        }

        protected override string Part2(string input)
        {
            var matches = input.GetLines<string>()
                .Select(l => Regex.Match(l))
                .ToArray();

            var memory = new Dictionary<ulong, ulong>();
            var mask = string.Empty;
            foreach (var match in matches)
            {
                if (match.Groups["mask"].Success)
                {
                    mask = match.Groups["mask"].Value;
                }
                else
                {
                    var address = Convert.ToString(Convert.ToInt32(match.Groups["address"].Value), 2)
                        .PadLeft(36, '0');
                    var result =
                        new string(address.Select((c, i) => mask[i] == '0' ? c : mask[i]).ToArray());
                    var addresses = GetAddresses(result);
                    var value = ulong.Parse(match.Groups["value"].Value);
                    foreach (var a in addresses)
                    {
                        memory[Convert.ToUInt64(a, 2)] = value;
                    }
                }
            }

            return memory.Values.Aggregate((a, c) => a + c).ToString();
        }

        private static IEnumerable<string> GetAddresses(string address)
        {
            var index = address.IndexOf('X');

            if (index == -1)
            {
                return MoreEnumerable.Return(address);
            }

            var replacements = new List<string>
            {
                address.Remove(index, 1).Insert(index, "0"),
                address.Remove(index, 1).Insert(index, "1")
            };
            return replacements.SelectMany(GetAddresses);
        }
    }
}