namespace AdventOfCode.Core.Test;

public abstract class TestBase<TSolution>
    where TSolution : ISolution
{
    protected TestBase() => this.Solution = Activator.CreateInstance<TSolution>();

    protected TSolution Solution { get; }
}
