using System.Text;

namespace Synnovation.WebFramework.Core;

public class HttpResponse(int statusCode, string body)
{
    public int StatusCode { get; set; } = statusCode;
    public string Body { get; set; } = body;
    public string ContentType { get; set; } = "text/html";
    public Dictionary<string, string> Headers { get; set; } = new();

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"HTTP/1.1 {StatusCode} OK");
        builder.AppendLine($"Content-Type: {ContentType}");
        builder.AppendLine($"Content-Length: {Encoding.UTF8.GetByteCount(Body)}");

        // Add custom headers
        foreach (var header in Headers)
        {
            builder.AppendLine($"{header.Key}: {header.Value}");
        }

        builder.AppendLine(); // Blank line to separate headers from the body
        builder.AppendLine(Body);

        return builder.ToString();
    }
}