namespace SlamCodeBlog.KeyedServices.Reflection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class ServiceKeyAttribute : Attribute
    {
        public ServiceKeyAttribute(string key, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Key = key;
            Lifetime = lifetime;
        }

        public string Key { get; }
        public ServiceLifetime Lifetime { get; }
    }
}
