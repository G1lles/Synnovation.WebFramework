using System.Reflection;
using System.Text.Json;
using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Controllers;
using Synnovation.WebFramework.Core.Types;

namespace Synnovation.WebFramework.Routing
{
    public static class RouteHandler
    {
        public static HttpResponse HandleRequest(HttpRequest request)
        {
            var matchedRoute = RouteTable.Instance.Routes.FirstOrDefault(r =>
                MatchPath(r.Path, request.Path) &&
                r.HttpMethod.Equals(request.Method, StringComparison.OrdinalIgnoreCase));

            if (matchedRoute == null)
            {
                // 404 or throw custom exception
                return new HttpResponse(404, "Route not found");
            }

            var controller = Activator.CreateInstance(matchedRoute.ControllerType);
            if (controller == null)
            {
                return new HttpResponse(500, "Controller not found");
            }

            var actionMethod = matchedRoute.ControllerType.GetMethod(matchedRoute.ActionName);
            if (actionMethod == null)
            {
                return new HttpResponse(500, "Action method not found");
            }

            // If it's a controller base, set the request property
            if (controller is MvcControllerBase mvcCtrl) mvcCtrl.Request = request;
            if (controller is ApiControllerBase apiCtrl) apiCtrl.Request = request;

            // Bind parameters
            var parameters = BindParameters(matchedRoute, request, actionMethod);

            // Invoke
            var result = actionMethod.Invoke(controller, parameters);

            // If result is IActionResult => produce HttpResponse
            if (result is IActionResult actionResult)
            {
                return actionResult.ExecuteResult();
            }

            // Fallback => string => text/html
            var stringBody = result?.ToString() ?? "";
            return new HttpResponse(200, stringBody) { ContentType = "text/html" };
        }

        private static object[] BindParameters(RouteConfig route, HttpRequest request, MethodInfo method)
        {
            // e.g. method might have signature: public IActionResult Update(int id, [FromBody] CreateUserDto body)
            var paramInfos = method.GetParameters();
            var boundParams = new object[paramInfos.Length];

            // Split route path / request path
            var routeSegments = route.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var requestSegments = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var routeParamIndex = 0; // track how many placeholders we've used

            for (var i = 0; i < paramInfos.Length; i++)
            {
                var pInfo = paramInfos[i];
                var hasFromBody = pInfo.GetCustomAttribute<FromBodyAttribute>() != null;

                if (hasFromBody)
                {
                    // 1. If param is [FromBody], parse JSON from request.Body
                    var bodyJson = request.Body;
                    if (string.IsNullOrWhiteSpace(bodyJson))
                    {
                        boundParams[i] = GetDefaultValue(pInfo.ParameterType);
                    }
                    else
                    {
                        try
                        {
                            var deserialized = JsonSerializer.Deserialize(bodyJson, pInfo.ParameterType);
                            boundParams[i] = deserialized ?? GetDefaultValue(pInfo.ParameterType);
                        }
                        catch
                        {
                            // Could handle bad JSON => 400 or throw
                            throw new Exception("Invalid JSON in request body.");
                        }
                    }
                }
                else
                {
                    // 2. Otherwise, see if the route path has a placeholder for this param
                    // Example: /api/users/{id} => requestSegments[2] might be "5"
                    // We'll assume the param is the next placeholder in routeSegments
                    // i.e. "api", "users", "{id}" => segment index 2 => "5"

                    // We'll find which route segment is a placeholder
                    // routeParamIndex is how many placeholders we've bound so far
                    // so let’s incrementally find the next {placeholder}.
                    var placeholders = routeSegments
                        .Where(seg => seg.StartsWith('{') && seg.EndsWith('}'))
                        .ToList();

                    if (routeParamIndex < placeholders.Count)
                    {
                        // find the index of that placeholder in routeSegments
                        var placeholderSegment = placeholders[routeParamIndex]; // e.g. "{id}"
                        var placeholderIndex = Array.IndexOf(routeSegments, placeholderSegment);

                        var rawValue = requestSegments[placeholderIndex]; // e.g. "2"

                        // Convert rawValue => correct type
                        boundParams[i] = ConvertToType(rawValue, pInfo.ParameterType);
                        routeParamIndex++;
                    }
                    else
                    {
                        boundParams[i] = GetDefaultValue(pInfo.ParameterType);
                    }
                }
            }

            return boundParams;
        }

        private static object? ConvertToType(string rawValue, Type targetType)
        {
            if (targetType != typeof(int)) return rawValue;
            
            if (int.TryParse(rawValue, out var intVal))
            {
                return intVal;
            }

            throw new Exception($"Expected an int but got '{rawValue}'");
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
}