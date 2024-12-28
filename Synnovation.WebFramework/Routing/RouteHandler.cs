using System.Reflection;
using System.Text.Json;
using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Controllers;
using Synnovation.WebFramework.Core.Types;
using Synnovation.WebFramework.Exceptions;

namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Handles routing and delegates requests to the appropriate controller and action method.
/// </summary>
public static class RouteHandler
{
    /// <summary>
    /// Matches a request to a route, invokes the appropriate controller and action, and returns a response.
    /// </summary>
    public static HttpResponse HandleRequest(HttpRequest request)
    {
        // Here we match the request to a route based on path and HTTP method
        var matchedRoute = RouteTable.Instance.Routes.FirstOrDefault(r =>
            MatchPath(r.Path, request.Path) &&
            r.HttpMethod.Equals(request.Method, StringComparison.OrdinalIgnoreCase));

        if (matchedRoute == null)
        {
            // 404 or throw custom exception
            throw new RouteNotFoundException("Route not found");
        }

        // We instantiate the specified controller for the matched route
        var controller = Activator.CreateInstance(matchedRoute.ControllerType);
        if (controller == null)
        {
            throw new ControllerNotFoundException("Controller not found");
        }

        // Find the action method in the controller for the matched route.
        var actionMethod = matchedRoute.ControllerType.GetMethod(matchedRoute.ActionName);
        if (actionMethod == null)
        {
            throw new ActionNotFoundException("Action not found");
        }

        // Pass the HttpRequest to the controller instance.
        if (controller is MvcControllerBase mvcCtrl) mvcCtrl.Request = request;
        if (controller is ApiControllerBase apiCtrl) apiCtrl.Request = request;

        // Bind parameters for the action method (e.g., route placeholders, JSON body).
        var parameters = BindParameters(matchedRoute, request, actionMethod);

        // Invoke the action method and capture the result.
        var result = actionMethod.Invoke(controller, parameters);

        // Convert the action result to an HttpResponse.
        if (result is IActionResult actionResult)
        {
            return actionResult.ExecuteResult();
        }

        // Fallback => string => text/html
        var stringBody = result?.ToString() ?? "";
        return new HttpResponse(200, stringBody) { ContentType = "text/html" };
    }

    /// <summary>
    /// Matches placeholders in the route to request parameters or body data.
    /// </summary>
    private static object[] BindParameters(RouteConfig route, HttpRequest request, MethodInfo method)
    {
        var paramInfos = method.GetParameters();
        var boundParams = new object[paramInfos.Length];

        var routeSegments = route.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var requestSegments = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var routeParamIndex = 0;

        for (var i = 0; i < paramInfos.Length; i++)
        {
            var pInfo = paramInfos[i];
            var hasFromBody = pInfo.GetCustomAttribute<FromBodyAttribute>() != null;
            var hasFromQuery = pInfo.GetCustomAttribute<FromQueryAttribute>() != null;

            if (hasFromBody)
            {
                // Parse the body for [FromBody]
                var bodyJson = request.Body;
                boundParams[i] = string.IsNullOrWhiteSpace(bodyJson)
                    ? GetDefaultValue(pInfo.ParameterType)!
                    : JsonSerializer.Deserialize(bodyJson, pInfo.ParameterType) ??
                      GetDefaultValue(pInfo.ParameterType)!;
            }
            else if (hasFromQuery)
            {
                // Parse query parameters for [FromQuery]
                if (request.QueryParameters.TryGetValue(pInfo.Name ?? "", out var queryValue) &&
                    !string.IsNullOrWhiteSpace(queryValue))
                {
                    boundParams[i] = ConvertToType(queryValue, pInfo.ParameterType)!;
                }
                else
                {
                    // Use default value if query parameter is missing
                    boundParams[i] = GetDefaultValue(pInfo.ParameterType)!;
                }
            }
            else
            {
                // Handle route placeholders (e.g., {id})
                var placeholders = routeSegments
                    .Where(seg => seg.StartsWith('{') && seg.EndsWith('}'))
                    .ToList();

                if (routeParamIndex < placeholders.Count)
                {
                    var placeholderSegment = placeholders[routeParamIndex];
                    var placeholderIndex = Array.IndexOf(routeSegments, placeholderSegment);

                    var rawValue = requestSegments[placeholderIndex];
                    boundParams[i] = ConvertToType(rawValue, pInfo.ParameterType)!;
                    routeParamIndex++;
                }
                else
                {
                    boundParams[i] = GetDefaultValue(pInfo.ParameterType)!;
                }
            }
        }

        return boundParams;
    }


    private static object? ConvertToType(string rawValue, Type targetType)
    {
        if (targetType == typeof(int) && int.TryParse(rawValue, out var intVal))
        {
            return intVal;
        }

        if (targetType == typeof(bool) && bool.TryParse(rawValue, out var boolVal))
        {
            return boolVal;
        }

        if (targetType == typeof(string))
        {
            return rawValue;
        }

        throw new Exception($"Cannot convert value '{rawValue}' to type {targetType.Name}.");
    }

    private static object? GetDefaultValue(Type t)
    {
        return t.IsValueType ? Activator.CreateInstance(t) : null;
    }

    private static bool MatchPath(string routePath, string requestPath)
    {
        var routeSegs = routePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var reqSegs = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (routeSegs.Length != reqSegs.Length)
            return false;

        return !routeSegs.Where((t, i) =>
                (!t.StartsWith('{') || !t.EndsWith('}')) &&
                !t.Equals(reqSegs[i], StringComparison.OrdinalIgnoreCase))
            .Any();
    }
}