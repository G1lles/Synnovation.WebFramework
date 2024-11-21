namespace Synnovation.WebFramework.Core;

public class HttpRequest
{
    public string Method { get; private set; }
    public string Path { get; private set; }
    public Dictionary<string, string> QueryParameters { get; private set; } = new();
    public string Body { get; private set; }

    public static HttpRequest Parse(string requestString)
    {
        var request = new HttpRequest();
        var lines = requestString.Split("\r\n");

        // Parse request line
        var requestLine = lines[0].Split(' ');
        request.Method = requestLine[0];
        request.Path = requestLine[1];
        if (request.Path.Contains("?"))
        {
            var parts = request.Path.Split('?');
            request.Path = parts[0];
            foreach (var kv in parts[1].Split('&'))
            {
                var kvParts = kv.Split('=');
                request.QueryParameters[kvParts[0]] = kvParts[1];
            }
        }

        // Parse body (if any)
        var bodyStart = Array.IndexOf(lines, "") + 1;
        if (bodyStart > 0 && bodyStart < lines.Length)
        {
            request.Body = string.Join("\r\n", lines[bodyStart..]);
        }

        return request;
    }
}