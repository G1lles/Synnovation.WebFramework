using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Exceptions;

namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Responsible for handling and executing routes.
/// </summary>
public static class RouteHandler
{
    public static HttpResponse HandleRequest(HttpRequest request)
    {
        // 1. Match the route by path and HTTP method
        var matchingRoute = RouteTable.Instance.Routes.FirstOrDefault(route =>
            MatchPath(route.Path, request.Path) &&
            string.Equals(route.HttpMethod, request.Method, StringComparison.OrdinalIgnoreCase));

        if (matchingRoute == null)
        {
            // Throw custom exception for no matching route
            throw new RouteNotFoundException($"No route found for {request.Method} {request.Path}");
        }

        // 2. Create controller and invoke action
        var controller = Activator.CreateInstance(matchingRoute.ControllerType);
        if (controller == null)
            throw new ControllerNotFoundException($"Controller '{matchingRoute.ControllerType.Name}' not found.");

        var action = matchingRoute.ControllerType.GetMethod(matchingRoute.ActionName);
        if (action == null)
            throw new ActionNotFoundException(
                $"Action '{matchingRoute.ActionName}' not found in controller '{matchingRoute.ControllerType.Name}'.");

        // If it's a ControllerBase, set the Request property
        if (controller is ControllerBase baseController)
        {
            baseController.Request = request;
        }

        // 3. Bind route parameters to action method
        var parameters = BindParameters(matchingRoute, request);

        // 4. Invoke the action
        var result = action.Invoke(controller, parameters.ToArray());

        // 5. Convert the action result to an HttpResponse
        return new HttpResponse(200, result?.ToString() ?? "");
    }

    private static bool MatchPath(string routePath, string requestPath)
    {
        var routeSegments = routePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var requestSegments = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (routeSegments.Length != requestSegments.Length)
            return false;

        for (int i = 0; i < routeSegments.Length; i++)
        {
            var routeSegment = routeSegments[i];
            var requestSegment = requestSegments[i];

            // If the route segment is a parameter (e.g., {id}), skip equality check
            if (routeSegment.StartsWith('{') && routeSegment.EndsWith('}'))
                continue;

            if (!string.Equals(routeSegment, requestSegment, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    private static object[] BindParameters(RouteConfig route, HttpRequest request)
    {
        var parameters = new List<object>();
        var routeSegments = route.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var requestSegments = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < routeSegments.Length; i++)
        {
            if (routeSegments[i].StartsWith('{') && routeSegments[i].EndsWith('}'))
            {
                parameters.Add(requestSegments[i]);
            }
        }

        // Add query parameters if action method defines them
        parameters.AddRange(
            from queryParam in route.ParameterNames
            where request.QueryParameters.ContainsKey(queryParam)
            select request.QueryParameters[queryParam]
        );

        return parameters.ToArray();
    }
}