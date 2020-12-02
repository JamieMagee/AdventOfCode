using System.Threading.Tasks;

namespace AdventOfCode._2020.Puzzles.Core
{
    public interface IProgressPublisher
    {
        bool IsUpdateProgressNeeded();
        Task UpdateProgressAsync(double current, double total);
    }
}