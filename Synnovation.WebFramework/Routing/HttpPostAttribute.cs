namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Attribute for HTTP POST requests.
/// </summary>
public class HttpPostAttribute : HttpVerbAttribute
{
    public HttpPostAttribute(string path) : base(path) { }
}