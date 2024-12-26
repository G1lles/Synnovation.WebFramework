namespace Synnovation.WebFramework.Annotations
{
    /// <summary>
    /// Indicates that a method parameter should be bound from the body of the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromBodyAttribute : Attribute
    {
    }
}