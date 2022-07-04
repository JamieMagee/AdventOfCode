namespace AdventOfCode.Core;

public interface IProgressPublisher
{
    bool IsUpdateProgressNeeded();

    Task UpdateProgressAsync(double current, double total);
}
