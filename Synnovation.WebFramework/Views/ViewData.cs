namespace Synnovation.WebFramework.Views;

/// <summary>
/// Stores data to be passed from controllers to views.
/// </summary>
public class ViewData
{
    public Dictionary<string, object> Data { get; } = new();

    /// <summary>
    /// Sets or gets the main model for the view.
    /// </summary>
    public object Model
    {
        get => Data.ContainsKey("Model") ? Data["Model"] : null;
        set => Data["Model"] = value;
    }

    /// <summary>
    /// Adds key-value pairs to the view data.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Add(string key, object value)
    {
        Data[key] = value;
    }

    public object this[string key]
    {
        get => Data.ContainsKey(key) ? Data[key] : null;
        set => Data[key] = value;
    }
}