namespace AdventOfCode.Web.Services;

public interface IInputHandler
{
    bool IsCachedInputAvailable(int day);

    Task<string> GetInputAsync(int year, int day);

    Task<string> GetSourceCodeAsync(int year, int day);

    object[] GetResults(int day);

    void ClearResults(int day);
}
