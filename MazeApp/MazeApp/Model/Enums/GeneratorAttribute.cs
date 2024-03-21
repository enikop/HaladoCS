using System;

namespace MazeApp.Model.Enums
{
    public class GeneratorAttribute : Attribute
    {
        public Type StrategyType { get; private set; }
        public IGenerationStrategy GenerationStrategy { get; private set; }
        public GeneratorAttribute(Type type)
        {
            if (!typeof(IGenerationStrategy).IsAssignableFrom(type))
            {
                throw new ArgumentException("Strategy type must implement IGenerationStrategy");
            }
            StrategyType = type;
            IGenerationStrategy? strategyObject = (IGenerationStrategy?)Activator.CreateInstance(StrategyType);
            if (strategyObject != null)
            {
                GenerationStrategy = strategyObject;
            }
            else
            {
                throw new ArgumentException("Strategy generation unsuccessful.");
            }
        }
    }
}
