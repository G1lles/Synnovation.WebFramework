using System.Text.RegularExpressions;

namespace Synnovation.WebFramework.Views;

public static partial class ViewEngine
{
    private static readonly string ViewsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
    
    public static string Render(string viewName, ViewData viewData)
    {
        var viewPath = Path.Combine(ViewsFolder, $"{viewName}.html");
        if (!File.Exists(viewPath))
            throw new FileNotFoundException($"View '{viewName}' not found at path: {viewPath}");

        var viewContent = File.ReadAllText(viewPath);

        // Process conditional rendering
        viewContent = ProcessConditionals(viewContent, viewData);

        // Process loops
        viewContent = ProcessLoops(viewContent, viewData);

        // Replace placeholders
        return viewData.Data.Keys.Aggregate(viewContent, (current, key) =>
            current.Replace($"{{{{ {key} }}}}", viewData.Data[key].ToString() ?? string.Empty));
    }

    private static string ProcessConditionals(string content, ViewData viewData)
    {
        var ifRegex = MyRegex1();
        return ifRegex.Replace(content, match =>
        {
            var condition = match.Groups[1].Value.Trim();
            var innerContent = match.Groups[2].Value;

            if (viewData.Data.TryGetValue(condition, out var conditionValue) && conditionValue is bool boolValue &&
                boolValue)
            {
                return innerContent;
            }

            return string.Empty; // If condition is false, remove the content
        });
    }

    private static string ProcessLoops(string content, ViewData viewData)
    {
        var loopRegex = MyRegex();
        return loopRegex.Replace(content, match =>
        {
            var itemName = match.Groups[1].Value.Trim();
            var collectionName = match.Groups[2].Value.Trim();
            var innerContent = match.Groups[3].Value;

            if (!viewData.Data.TryGetValue(collectionName, out var collectionValue) ||
                collectionValue is not IEnumerable<object> collection)
                return string.Empty; // If collection is null or empty

            var renderedContent = new StringWriter();
            foreach (var item in collection)
            {
                var itemContent = innerContent.Replace($"{{{{ {itemName} }}}}", item.ToString());
                renderedContent.Write(itemContent);
            }

            return renderedContent.ToString();
        });
    }

    [GeneratedRegex(@"{{#foreach (.*?) in (.*?)}}(.*?){{/foreach}}", RegexOptions.Singleline)]
    private static partial Regex MyRegex();

    [GeneratedRegex(@"{{#if (.*?)}}(.*?){{/if}}", RegexOptions.Singleline)]
    private static partial Regex MyRegex1();
}