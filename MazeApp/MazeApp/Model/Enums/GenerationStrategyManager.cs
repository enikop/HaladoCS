using System;
using System.Linq;

namespace MazeApp.Model.Enums
{
    public class GenerationStrategyManager
    {
        public static IGenerationStrategy GetStrategy(GenerationAlgorithm generationAlgorithm)
        {
            Type type = typeof(GenerationAlgorithm);
            GeneratorAttribute? attr = (GeneratorAttribute?)type.GetField(generationAlgorithm.ToString())?.GetCustomAttributes(typeof(GeneratorAttribute), false).SingleOrDefault();
            if (attr != null)
            {
                return attr.GenerationStrategy;
            }
            else
            {
                throw new ArgumentException("No definition found for given generation strategy.");
            }
        }
    }
}
