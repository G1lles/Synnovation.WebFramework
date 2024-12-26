using System.Text.Json;
using Synnovation.WebFramework.Core.Types;

namespace Synnovation.WebFramework.Core;

/// <summary>
/// Returns JSON with a given status code.
/// </summary>
public class JsonResult(object? data, int statusCode = 200) : IActionResult
{
    public HttpResponse ExecuteResult()
    {
        var json = JsonSerializer.Serialize(data ?? new { });
        return new HttpResponse(statusCode, json)
        {
            ContentType = "application/json"
        };
    }
}