using Synnovation.WebFramework.Core;

namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Responsible for handling and executing routes.
/// </summary>
public static class RouteHandler
{
    public static HttpResponse HandleRequest(HttpRequest request)
    {
        // Match the route by path and HTTP method
        var matchingRoute = RouteTable.Instance.Routes.FirstOrDefault(route =>
            MatchPath(route.Path, request.Path) &&
            string.Equals(route.HttpMethod, request.Method, StringComparison.OrdinalIgnoreCase));

        if (matchingRoute == null)
        {
            return new HttpResponse(404, "Route not found");
        }

        // Create controller and invoke action
        var controller = Activator.CreateInstance(matchingRoute.ControllerType);
        var action = matchingRoute.ControllerType.GetMethod(matchingRoute.ActionName);

        if (controller == null || action == null)
        {
            return new HttpResponse(500, "Controller or action not found");
        }

        // Bind route parameters to action method
        var parameters = BindParameters(matchingRoute, request);
        var result = action.Invoke(controller, parameters.ToArray());

        // Return result as response
        return new HttpResponse(200, result?.ToString() ?? "");
    }

    private static bool MatchPath(string routePath, string requestPath)
    {
        var routeSegments = routePath.Split('/');
        var requestSegments = requestPath.Split('/');

        if (routeSegments.Length != requestSegments.Length)
            return false;

        return !routeSegments.Where((t, i) =>
            (!t.StartsWith('{') || !t.EndsWith('}')) &&
            !string.Equals(t, requestSegments[i], StringComparison.OrdinalIgnoreCase)).Any();
    }

    private static object[] BindParameters(RouteConfig route, HttpRequest request)
    {
        var parameters = new List<object>();
        var routeSegments = route.Path.Split('/');
        var requestSegments = request.Path.Split('/');

        for (var i = 0; i < routeSegments.Length; i++)
        {
            if (routeSegments[i].StartsWith('{') && routeSegments[i].EndsWith('}'))
            {
                parameters.Add(requestSegments[i]);
            }
        }

        // Add query parameters if action method defines them
        parameters.AddRange((from queryParam in route.ParameterNames
            where request.QueryParameters.ContainsKey(queryParam)
            select request.QueryParameters[queryParam]).Cast<object>());

        return parameters.ToArray();
    }
}