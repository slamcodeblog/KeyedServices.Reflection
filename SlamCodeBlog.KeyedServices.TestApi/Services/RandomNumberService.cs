namespace SlamCodeBlog.KeyedServices.TestApi.Services
{
    public class RandomNumberService : IRandomNumberService
    {
        public virtual int Next() => Random.Shared.Next();
    }
}
