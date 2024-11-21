namespace Synnovation.WebFramework.Builder;

/// <summary>
/// A lightweight dependency injection container.
/// </summary>
public class ServiceCollection : IServiceCollection
{
    private readonly Dictionary<Type, object> _singletons = new();

    public void AddSingleton<TService, TImplementation>() where TImplementation : TService
    {
        _singletons[typeof(TService)] = Activator.CreateInstance(typeof(TImplementation))!;
    }

    public void AddSingleton<TService>(TService implementation)
    {
        _singletons[typeof(TService)] = implementation;
    }

    public TService GetService<TService>()
    {
        return (TService)_singletons[typeof(TService)];
    }
}