using SlamCodeBlog.KeyedServices.Reflection;
using System.Reflection;

namespace SlamCode.DependencyInjection.Reflection
{
    internal class InjectionServiceDescriptor
    {
        private InjectionServiceDescriptor(
            Type serviceType, 
            Type implementationType, 
            IEnumerable<InjectionConstructor> constructors,
            object? key)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            Constructors = constructors;
            Key = key;
            UsesKeyedDependencies = constructors.Any(ic => ic.HasKeyedServiceParameter);
        }

        public object? Key { get; }
        public Type ServiceType { get; }
        public Type ImplementationType { get; }
        public IEnumerable<InjectionConstructor> Constructors { get; }
        public bool UsesKeyedDependencies { get; }

        public object Resolve(IServiceCollection services, IServiceProvider provider)
        {
            var resolveableConstructor = Constructors.FirstOrDefault(ic => ic.CanBeResolved(services));

            return resolveableConstructor == null
                ? throw new InvalidOperationException($"No constructors found for type '{ImplementationType.FullName}', that can be resolved using given provider. Make sure you registered all the required dependencies.")
                : resolveableConstructor.Resolve(provider);
        }

        public static InjectionServiceDescriptor Create<TService, TImplementation>() 
        {
            var implType = typeof(TImplementation);
            return new(typeof(TService), implType, 
                implType.GetConstructors()
                    .Select(InjectionConstructor.Create),
                implType.GetCustomAttribute<ServiceKeyAttribute>()?.Key
                );
        }
    }
}
