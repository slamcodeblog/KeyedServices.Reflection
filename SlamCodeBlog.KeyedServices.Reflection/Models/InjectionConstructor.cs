using System.Reflection;

namespace SlamCodeBlog.KeyedServices.Reflection.Models
{
    internal record InjectionConstructor
    {
        public Type ConstructedType { get; private set; }
        public int ParametersCount { get; private set; }
        public IEnumerable<InjectionParameter> Parameters { get; private set; }
        public bool HasKeyedServiceParameter { get; private set; }
        public bool IsActivatorConstructor { get; private set; }

        private InjectionConstructor(Type constructedType, IReadOnlyList<InjectionParameter> parameters)
        {
            ConstructedType = constructedType;
            ParametersCount = parameters.Count;
            Parameters = parameters;
            HasKeyedServiceParameter = parameters.Any(ip => ip.Keyed);
            IsActivatorConstructor = constructedType.GetCustomAttribute<ActivatorUtilitiesConstructorAttribute>() != null;
        }

        public bool CanBeResolved(IServiceCollection services) => Parameters.All(p => p.CanBeResolved(services));

        public object Resolve(IServiceProvider serviceProvider) => Activator.CreateInstance(
            ConstructedType, Parameters.Select(p => p.Resolve(serviceProvider)).ToArray())!;

        internal static InjectionConstructor Create(ConstructorInfo constructorInfo)
        {
            if (!constructorInfo.IsPublic)
            {
                throw new InvalidOperationException("Can not inject non public constructor.");
            }
            return new(constructorInfo.ReflectedType!, constructorInfo.GetParameters().Select(InjectionParameter.Create).ToList().AsReadOnly());
        }
    }
}
