namespace AdventOfCode.Web.Visualizers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class TargetSolutionAttribute : Attribute
{
    public TargetSolutionAttribute(Type targetSolutionType) => this.TargetSolutionType = targetSolutionType;

    public Type TargetSolutionType { get; }
}
