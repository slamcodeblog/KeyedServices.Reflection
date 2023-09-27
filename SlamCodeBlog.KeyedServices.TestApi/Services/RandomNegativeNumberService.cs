namespace SlamCode.DependencyInjection.Reflection.TestApi.Services
{
    [ServiceKey("Negative")]
    public class RandomNegativeNumberService : IRandomNumberService
    {
        private readonly IServiceProvider serviceProvider;

        public RandomNegativeNumberService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public int Next() => Random.Shared.Next(int.MinValue, -1);
    }
}
