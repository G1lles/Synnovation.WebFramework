using System;
using System.IO;

namespace Synnovation.WebFramework.Views;

/// <summary>
/// Responsible for rendering views with dynamic content.
/// </summary>
public static class ViewEngine
{
    private static string _viewsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");

    /// <summary>
    /// Configures the folder where views are located.
    /// </summary>
    /// <param name="viewsFolder">Path to the folder containing views.</param>
    public static void Configure(string viewsFolder)
    {
        // Convert relative path to absolute
        var absolutePath = Path.IsPathRooted(viewsFolder)
            ? viewsFolder
            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, viewsFolder);

        if (!Directory.Exists(absolutePath))
            throw new DirectoryNotFoundException($"Views folder not found: {absolutePath}");

        _viewsFolder = absolutePath;
    }

    /// <summary>
    /// Renders a view by name using data.
    /// </summary>
    /// <param name="viewName">The name of the view file (without extension).</param>
    /// <param name="viewData">Data to render in the view.</param>
    /// <returns>Rendered HTML content.</returns>
    public static string Render(string viewName, ViewData viewData)
    {
        var viewPath = Path.Combine(_viewsFolder, $"{viewName}.html");
        if (!File.Exists(viewPath))
            throw new FileNotFoundException($"View '{viewName}' not found at path: {viewPath}");

        var viewContent = File.ReadAllText(viewPath);

        // Replace placeholders with data
        return viewData.Data.Keys.Aggregate(viewContent, (current, key) => current.Replace($"{{{{ {key} }}}}", viewData.Data[key]?.ToString() ?? string.Empty));
    }
}