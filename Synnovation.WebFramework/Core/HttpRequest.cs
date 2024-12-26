namespace Synnovation.WebFramework.Core;

public class HttpRequest
{
    public string Method { get; set; } = "";
    public string Path { get; set; } = "";
    public string Body { get; set; } = "";

    public Dictionary<string, string> QueryParameters { get; set; } = new();
    public Dictionary<string, string> Headers { get; set; } = new();

    // Optional form dictionary if using form parsing
    public Dictionary<string, string> Form { get; set; } = new();

    public static HttpRequest Parse(string requestString)
    {
        var request = new HttpRequest();
        var lines = requestString.Split("\r\n");

        // Parse request line
        var requestLine = lines[0].Split(' ');
        request.Method = requestLine[0];
        request.Path = requestLine[1];

        if (request.Path.Contains('?'))
        {
            var parts = request.Path.Split('?');
            request.Path = parts[0];
            foreach (var kv in parts[1].Split('&'))
            {
                var kvParts = kv.Split('=');
                if (kvParts.Length == 2)
                    request.QueryParameters[kvParts[0]] = kvParts[1];
            }
        }

        // Parse headers
        var headerEndIndex = Array.IndexOf(lines, "");
        if (headerEndIndex < 0) headerEndIndex = lines.Length; // fallback
        for (var i = 1; i < headerEndIndex; i++)
        {
            var headerParts = lines[i].Split(": ", 2, StringSplitOptions.RemoveEmptyEntries);
            if (headerParts.Length == 2)
            {
                request.Headers[headerParts[0]] = headerParts[1];
            }
        }

        // Parse body (if any)
        var bodyStart = headerEndIndex + 1;
        if (bodyStart > 0 && bodyStart < lines.Length)
        {
            request.Body = string.Join("\r\n", lines[bodyStart..]);
        }

        return request;
    }
}
