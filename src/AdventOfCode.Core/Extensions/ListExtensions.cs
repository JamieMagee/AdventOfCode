namespace AdventOfCode.Core.Extensions;

public static class ListExtensions
{
    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (list.Count < 1)
        {
            throw new ArgumentException("List must contain at least one element.", nameof(list));
        }

        first = list[0];
        rest = [.. list.Skip(1)];
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
    {
        ArgumentNullException.ThrowIfNull(list);
        if (list.Count < 2)
        {
            throw new ArgumentException("List must contain at least two elements.", nameof(list));
        }

        first = list[0];
        second = list[1];
        rest = [.. list.Skip(2)];
    }
}
