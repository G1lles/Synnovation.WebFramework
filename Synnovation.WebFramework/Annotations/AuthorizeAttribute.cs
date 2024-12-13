namespace Synnovation.WebFramework.Annotations;

/// <summary>
/// Specifies that the action/controller requires authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class AuthorizeAttribute : Attribute
{
}