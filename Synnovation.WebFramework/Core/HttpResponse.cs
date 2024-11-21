using System.Text;

namespace Synnovation.WebFramework.Core;

public class HttpResponse
{
    public int StatusCode { get; set; }
    public string Body { get; set; }
    public string ContentType { get; set; } = "text/html";

    public HttpResponse(int statusCode, string body)
    {
        StatusCode = statusCode;
        Body = body;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"HTTP/1.1 {StatusCode} OK");
        builder.AppendLine($"Content-Type: {ContentType}");
        builder.AppendLine($"Content-Length: {Encoding.UTF8.GetByteCount(Body)}");
        builder.AppendLine();
        builder.AppendLine(Body);
        return builder.ToString();
    }
}