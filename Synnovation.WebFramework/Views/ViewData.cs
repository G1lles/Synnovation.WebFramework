namespace Synnovation.WebFramework.Views;

/// <summary>
/// Stores data to be passed from controllers to views.
/// </summary>
public class ViewData
{
    public Dictionary<string, object> Data { get; } = new();

    public object Model
    {
        get => Data.GetValueOrDefault("Model") ?? null!;
        set => Data["Model"] = value;
    }

    public object this[string key]
    {
        get => Data.ContainsKey(key) ? Data[key] : null!;
        set => Data[key] = value;
    }
}