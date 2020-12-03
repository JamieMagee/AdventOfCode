using System;

namespace AdventOfCode._2020.Web.Visualizers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class TargetSolutionAttribute : Attribute
    {
        public Type TargetSolutionType { get; }
        
        public TargetSolutionAttribute(Type targetSolutionType)
        {
            TargetSolutionType = targetSolutionType;
        }
    }
}