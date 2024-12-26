namespace Synnovation.WebFramework.Annotations;

/// <summary>
/// Base class for HTTP verb attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class HttpVerbAttribute(string path) : Attribute
{
    public string Path { get; } = path;
}