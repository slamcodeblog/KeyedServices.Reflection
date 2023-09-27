using Scrutor;
using SlamCodeBlog.KeyedServices.Reflection;
using SlamCodeBlog.KeyedServices.Reflection.Models;
using System.Reflection;

namespace SlamCode.DependencyInjection.Reflection
{
    public static class KeyedServceCollectionExtensions
    {
        public static IServiceCollection AddWithKeyedDependencies<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptor = InjectionServiceDescriptor.Create<TService, TImplementation>();
            if(descriptor.UsesKeyedDependencies)
                services.Add(
                    new ServiceDescriptor(
                        typeof(TService), 
                        (sp) => descriptor.Resolve(services, sp),
                        lifetime));
            else services.Add(
                    new ServiceDescriptor(
                        typeof(TService),
                        typeof(TImplementation),
                        lifetime));

            return services;
        }

        public static IServiceCollection AddWithKeyedDependencies<TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TImplementation : class
            => AddWithKeyedDependencies<TImplementation, TImplementation>(services, lifetime);

        public static IServiceCollection AddTransientWithKeyedDependencies<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService => services.AddWithKeyedDependencies<TService, TImplementation>(ServiceLifetime.Transient);

        public static IServiceCollection AddScopedWithKeyedDependencies<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService => services.AddWithKeyedDependencies<TService, TImplementation>(ServiceLifetime.Scoped);

        public static IServiceCollection AddSingletonWithKeyedDependencies<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService => services.AddWithKeyedDependencies<TService, TImplementation>(ServiceLifetime.Singleton);

        public static IServiceCollection AddTransientWithKeyedDependencies<TImplementation>(this IServiceCollection services)
            where TImplementation : class => services.AddWithKeyedDependencies<TImplementation>(ServiceLifetime.Transient);

        public static IServiceCollection AddScopedWithKeyedDependencies<TImplementation>(this IServiceCollection services)
            where TImplementation : class => services.AddWithKeyedDependencies<TImplementation>(ServiceLifetime.Scoped);

        public static IServiceCollection AddSingletonWithKeyedDependencies<TImplementation>(this IServiceCollection services)
            where TImplementation : class => services.AddWithKeyedDependencies<TImplementation>(ServiceLifetime.Singleton);

        public static IServiceCollection AddAllKeyedServices(this IServiceCollection services) =>
            services.Scan(ts => ts.FromApplicationDependencies()
                .AddClasses(cls => cls.WithAttribute<ServiceKeyAttribute>()
                .Where(type => type.GetCustomAttribute<ServiceKeyAttribute>()!.Lifetime == ServiceLifetime.Transient))
                    .AsKeyedService().WithTransientLifetime())
            .Scan(ts => ts.FromApplicationDependencies()
                .AddClasses(cls => cls.WithAttribute<ServiceKeyAttribute>()
                .Where(type => type.GetCustomAttribute<ServiceKeyAttribute>()!.Lifetime == ServiceLifetime.Scoped))
                    .AsKeyedService().WithScopedLifetime())
            .Scan(ts => ts.FromApplicationDependencies()
                .AddClasses(cls => cls.WithAttribute<ServiceKeyAttribute>()
                .Where(type => type.GetCustomAttribute<ServiceKeyAttribute>()!.Lifetime == ServiceLifetime.Singleton))
                    .AsKeyedService().WithSingletonLifetime());

        public static ILifetimeSelector AsKeyedService(this IServiceTypeSelector serviceTypeSelector) => 
            serviceTypeSelector.UsingRegistrationStrategy(RegistrationStrategy.Append)
                .AsSelf()
                .AsImplementedInterfaces();

        public static bool UsesKeyedDependency(this ConstructorInfo cstr) =>
            cstr.GetParameters().Any(param => param.GetCustomAttribute<ServiceKeyAttribute>() is not null);
    }
}
