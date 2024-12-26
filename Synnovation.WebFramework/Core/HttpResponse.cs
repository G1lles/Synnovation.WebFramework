using System.Text;

namespace Synnovation.WebFramework.Core;

public class HttpResponse(int statusCode, string body)
{
    public int StatusCode { get; set; } = statusCode;
    public string Body { get; set; } = body;
    public string ContentType { get; set; } = "text/html";
    public Dictionary<string, string> Headers { get; set; } = new();

    private string ReasonPhrase { get; set; } = GetDefaultReasonPhrase(statusCode);

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"HTTP/1.1 {StatusCode} {ReasonPhrase}");
        builder.AppendLine($"Content-Type: {ContentType}");
        builder.AppendLine($"Content-Length: {Encoding.UTF8.GetByteCount(Body)}");

        foreach (var header in Headers)
        {
            builder.AppendLine($"{header.Key}: {header.Value}");
        }

        builder.AppendLine();
        builder.AppendLine(Body);

        return builder.ToString();
    }

    private static string GetDefaultReasonPhrase(int statusCode)
    {
        // You can expand this dictionary for more codes
        return statusCode switch
        {
            200 => "OK",
            201 => "Created",
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unknown"
        };
    }
}