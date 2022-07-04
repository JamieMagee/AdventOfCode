namespace AdventOfCode.Core;

public sealed class SolutionMetadata
{
    public SolutionMetadata(Type type, int year, int day, string name)
    {
        this.Type = type;
        this.Year = year;
        this.Day = day;
        this.Title = name;
    }

    public int Year { get; }

    public int Day { get; }

    public Type Type { get; }

    public string Title { get; }

    public ISolution CreateInstance() => (ISolution)Activator.CreateInstance(this.Type);
}
