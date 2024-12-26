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

        // Conditionals, loops, placeholders, etc.
        viewContent = ProcessConditionals(viewContent, viewData);
        viewContent = ProcessLoops(viewContent, viewData);

        foreach (var key in viewData.Data.Keys)
        {
            viewContent = viewContent.Replace($"{{{{ {key} }}}}", viewData.Data[key]?.ToString() ?? "");
        }

        return viewContent;
    }

    private static string ProcessConditionals(string content, ViewData viewData)
    {
        var ifRegex = MyRegex1();
        return ifRegex.Replace(content, match =>
        {
            var condition = match.Groups[1].Value.Trim();
            var innerContent = match.Groups[2].Value;

            if (viewData.Data.TryGetValue(condition, out var conditionValue)
                && conditionValue is bool boolValue
                && boolValue)
            {
                return innerContent;
            }

            return "";
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

            if (!viewData.Data.TryGetValue(collectionName, out var collectionValue)
                || collectionValue is not IEnumerable<object> collection)
            {
                return "";
            }

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