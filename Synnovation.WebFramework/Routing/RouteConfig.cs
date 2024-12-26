namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Represents a route configuration for the framework.
/// </summary>
public class RouteConfig
{
    public string Path { get; set; }
    public string HttpMethod { get; set; }
    public Type ControllerType { get; set; }
    public string ActionName { get; set; }
    public bool RequiresAuthorization { get; }
    private List<string> ParameterNames { get; set; } = [];

    public RouteConfig(
        string path,
        string httpMethod,
        Type controllerType,
        string actionName,
        bool requiresAuthorization)
    {
        Path = path.ToLowerInvariant();
        HttpMethod = httpMethod.ToUpperInvariant();
        ControllerType = controllerType;
        ActionName = actionName;
        RequiresAuthorization = requiresAuthorization;

        // Identify {param} placeholders in the path
        foreach (var segment in path.Split('/'))
        {
            if (segment.StartsWith('{') && segment.EndsWith('}'))
            {
                ParameterNames.Add(segment.Trim('{', '}'));
            }
        }
    }
}