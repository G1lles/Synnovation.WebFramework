namespace Synnovation.WebFramework.Annotations;

/// <summary>
/// Attribute for HTTP GET requests.
/// </summary>
public class HttpGetAttribute : HttpVerbAttribute
{
    public HttpGetAttribute(string path) : base(path) { }
}