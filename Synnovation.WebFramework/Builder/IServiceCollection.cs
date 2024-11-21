namespace Synnovation.WebFramework.Builder;

/// <summary>
/// Interface for registering and retrieving services.
/// </summary>
public interface IServiceCollection
{
    void AddSingleton<TService, TImplementation>() where TImplementation : TService;
    void AddSingleton<TService>(TService implementation);
    TService GetService<TService>();
}