namespace MazeApp.Model.Enums
{
    public enum GenerationAlgorithm
    {
        [Generator(typeof(TesselationStrategy))]
        Tesselation,
        [Generator(typeof(RecursiveDivideStrategy))]
        RecursiveDivide,
        [Generator(typeof(WilsonStrategy))]
        Wilson
    }
}
