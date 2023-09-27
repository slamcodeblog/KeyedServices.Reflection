using SlamCode.DependencyInjection.Reflection.TestApi.Services.Api;

namespace SlamCode.DependencyInjection.Reflection.TestApi.Services
{
    [ServiceKey("Positive")]
    public class RandomPositiveNumberService : IRandomNumberService
    {
        private readonly IRandomNumberService negativeNumberService;
        private readonly IApiService apiService;

        public RandomPositiveNumberService(RandomNegativeNumberService negativeNumberService,
            [ServiceKey("v2")] IApiService apiService)
        {
            this.negativeNumberService = negativeNumberService;
            this.apiService = apiService;
        }

        public int Next() => -1 * negativeNumberService.Next();
    }
}
