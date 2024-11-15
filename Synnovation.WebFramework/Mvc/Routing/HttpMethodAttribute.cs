namespace Synnovation.WebFramework.Mvc.Routing;

/**
 *
 */
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class HttpMethodAttribute : Attribute
{
    private readonly List<string> _httpMethods;
    private int? _order;

    protected HttpMethodAttribute(List<string> httpMethods)
    {
        ArgumentNullException.ThrowIfNull(httpMethods);
        _httpMethods = httpMethods.ToList();
    }

    public IEnumerable<string> HttpMethods => _httpMethods;

    public int Order
    {
        get => _order ?? 0;
        set => _order = value;
    }
}