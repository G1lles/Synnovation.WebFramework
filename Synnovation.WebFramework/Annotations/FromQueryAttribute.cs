namespace Synnovation.WebFramework.Annotations;

/// <summary>
/// Indicates that the parameter should be bound from query string values.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class FromQueryAttribute : Attribute;