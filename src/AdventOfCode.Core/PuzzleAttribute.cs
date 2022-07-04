namespace AdventOfCode.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PuzzleAttribute : Attribute
{
    public PuzzleAttribute(string title = null, int day = -1)
    {
        this.Day = day < 0 ? null : day;
        this.Title = title;
    }

    public int? Day { get; }

    public string Title { get; }
}
