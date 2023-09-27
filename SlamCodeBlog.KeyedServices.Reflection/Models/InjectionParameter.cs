using SlamCodeBlog.KeyedServices.Reflection;
using System.Reflection;

namespace SlamCodeBlog.KeyedServices.Reflection.Models
{
    internal record InjectionParameter
    {
        private Type? resolvingType;

        public Type ParameterType { get; private set; }

        public object? ServiceKey { get; private set; }

        public bool Keyed => ServiceKey is not null;

        private InjectionParameter(Type parameterType, object? serviceKey = null)
        {
            ParameterType = parameterType;
            ServiceKey = serviceKey;
        }

        public bool CanBeResolved(IServiceCollection services) => TryExtractResolvingType(services);

        public object Resolve(IServiceProvider serviceProvider) => serviceProvider.GetRequiredService(
            resolvingType
            ?? throw new InvalidOperationException($"Keyed dependency resolving type could not be determined for parameter of type '{ParameterType.FullName}'. Make sure you provided equal keys for service and parameter."));

        private bool TryExtractResolvingType(IServiceCollection services)
        {
            if (resolvingType != null)
            {
                return true;
            }

            if (!Keyed)
            {
                resolvingType = ParameterType;
                return true;
            }

            var implementations = services.Where(sd => ParameterType.IsAssignableFrom(sd.ServiceType)).ToList();
            var implementationsWithKey = implementations
                .Where(sd => sd.ImplementationType?.GetCustomAttribute<ServiceKeyAttribute>() is not null)
                .ToList();

            resolvingType = implementationsWithKey
                .FirstOrDefault(sd => Equals(sd.ImplementationType?.GetCustomAttribute<ServiceKeyAttribute>()?.Key, ServiceKey))
                ?.ImplementationType;

            return resolvingType != null;
        }

        internal static InjectionParameter Create(ParameterInfo parameterInfo) =>
            new(parameterInfo.ParameterType, parameterInfo.GetCustomAttribute<ServiceKeyAttribute>()?.Key);

    }
}
