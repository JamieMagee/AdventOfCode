using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Core.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<T> GetLines<T>(this string input)
        {
            return input.Replace("\r", string.Empty)
                .Split('\n')
                .Reverse()
                .SkipWhile(string.IsNullOrEmpty)
                .Reverse()
                .Select(s => (T) Convert.ChangeType(s, typeof(T)));
        }

        public static char? Get(this string[] map, (int, int) pos)
        {
            var (r, c) = pos;
            if (r < 0 || c < 0 || r >= map.Length || c >= map[r].Length)
            {
                return null;
            }

            return map[r][c];
        }
    }
}