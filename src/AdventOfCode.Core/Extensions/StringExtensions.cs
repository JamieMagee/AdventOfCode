namespace AdventOfCode.Core.Extensions;

using System.Globalization;

public static class StringExtensions
{
    public static IEnumerable<T> GetLines<T>(this string input) => input.Replace("\r", string.Empty)
        .Split('\n')
        .Reverse()
        .SkipWhile(string.IsNullOrEmpty)
        .Reverse()
        .Select(s => (T)Convert.ChangeType(s, typeof(T), CultureInfo.InvariantCulture));

    public static char? TryGetCharAt(this string input, int index)
    {
        if (index < 0 || index >= input.Length)
        {
            return null;
        }

        return input[index];
    }

    public static char? Get(this string[] map, (int R, int C) pos)
    {
        var (r, c) = pos;
        if (r < 0 || c < 0 || r >= map.Length || c >= map[r].Length)
        {
            return null;
        }

        return map[r][c];
    }
}
