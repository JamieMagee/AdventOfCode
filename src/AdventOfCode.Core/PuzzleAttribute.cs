namespace AdventOfCode.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PuzzleAttribute(string title, int day = -1) : Attribute
{
    public int? Day { get; } = day < 0 ? null : day;

    public string Title { get; } = title;
}
