namespace Synnovation.WebFramework.Annotations;

/// <summary>
/// Base class for HTTP verb attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class HttpVerbAttribute : Attribute
{
    public string Path { get; }

    public HttpVerbAttribute(string path)
    {
        Path = path;
    }
}