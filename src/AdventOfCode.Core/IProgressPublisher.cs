namespace AdventOfCode.Core;

public interface IProgressPublisher
{
    public bool IsUpdateProgressNeeded();

    public Task UpdateProgressAsync(double current, double total);
}
