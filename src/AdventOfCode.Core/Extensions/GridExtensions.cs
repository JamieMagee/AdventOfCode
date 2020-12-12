namespace AdventOfCode.Core.Extensions
{
    public static class GridExtensions
    {
        public static (int, int) Add(this (int, int) a, (int, int) b)
        {
            return (a.Item1 + b.Item1, a.Item2 + b.Item2);
        }
    }
}