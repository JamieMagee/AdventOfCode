using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AdventOfCode._2020.Web.Visualizers;

namespace AdventOfCode._2020.Web.Services
{
    public interface IVisualizerHandler
    {
        Type GetVisualizer(Type solutionType);

        void CancelAllVisualizations();

        CancellationToken GetVisualizationCancellationToken();
    }

    public sealed class VisualizerHandler : IVisualizerHandler
    {
        public IReadOnlyDictionary<Type, Type> VisualizersBySolutionType { get; }

        public VisualizerHandler()
        {
            VisualizersBySolutionType = GatherPuzzleSolutions();
        }

        public Type GetVisualizer(Type solutionType)
        {
            if (VisualizersBySolutionType.TryGetValue(solutionType, out var visualizerType))
            {
                return visualizerType;
            }
            return null;
        }

        private static Dictionary<Type, Type> GatherPuzzleSolutions()
        {
            var visualizersBySolutionType = new Dictionary<Type, Type>();
            var visualizerInterface = typeof(IVisualizer);
            var visualizerTypes = visualizerInterface.Assembly.GetTypes()
                .Where(x => visualizerInterface.IsAssignableFrom(x) && !x.IsAbstract)
                .ToList();

            foreach (var visualizerType in visualizerTypes)
            {
                var targetSolutionAttributes = visualizerType.GetCustomAttributes(typeof(TargetSolutionAttribute), false).OfType<TargetSolutionAttribute>().ToList();
                foreach (var targetSolutionType in targetSolutionAttributes.Select(x => x.TargetSolutionType))
                {
                    visualizersBySolutionType[targetSolutionType] = visualizerType;
                }
            }

            return visualizersBySolutionType;
        }

        public void CancelAllVisualizations()
        {
            myCancellationTokenSource.Cancel();
            myCancellationTokenSource = new CancellationTokenSource();
        }

        public CancellationToken GetVisualizationCancellationToken() => myCancellationTokenSource.Token;

        private CancellationTokenSource myCancellationTokenSource = new CancellationTokenSource();
    }
}