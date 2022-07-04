namespace AdventOfCode.Core.Extensions;

public static class GridExtensions
{
    public static (int X, int Y) Add(this (int X, int Y) a, (int X, int Y) b) => (a.X + b.X, a.Y + b.Y);
}
