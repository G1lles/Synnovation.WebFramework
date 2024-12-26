namespace Synnovation.WebFramework.Core.Types;

/// <summary>
/// Represents a typesafe result from a controller action.
/// </summary>
public interface IActionResult
{
    /// <summary>
    /// Produces the final HttpResponse.
    /// </summary>
    HttpResponse ExecuteResult();
}