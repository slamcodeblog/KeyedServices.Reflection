namespace SlamCode.DependencyInjection.Reflection.TestApi.Services
{
    public class WeatherGeneratorService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IRandomNumberService randomNumberService;

        public WeatherGeneratorService([ServiceKey("Positive")] IRandomNumberService randomNumberService)
        {
            this.randomNumberService = randomNumberService;
        }

        public int GetRandomTemperature() => randomNumberService.Next();

        public string GetRandomSummary() => Summaries[randomNumberService.Next() % Summaries.Length];
    }
}
