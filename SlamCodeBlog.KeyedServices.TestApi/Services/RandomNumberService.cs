namespace SlamCode.DependencyInjection.Reflection.TestApi.Services
{
    public class RandomNumberService : IRandomNumberService
    {
        public virtual int Next() => Random.Shared.Next();
    }
}
