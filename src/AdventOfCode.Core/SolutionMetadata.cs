namespace AdventOfCode.Core;

public sealed class SolutionMetadata(Type type, int year, int day, string name)
{
    public int Year { get; } = year;

    public int Day { get; } = day;

    public Type Type { get; } = type;

    public string Title { get; } = name;

    public ISolution CreateInstance() => (ISolution)Activator.CreateInstance(this.Type)!;
}
