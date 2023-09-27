namespace SlamCode.DependencyInjection.Reflection.TestApi.Services
{
    [ServiceKey("NonNegative")]
    public class RandomNonNegativeNumberService : IRandomNumberService
    {
        public int Next() => Random.Shared.Next(0, int.MaxValue);
    }
}
