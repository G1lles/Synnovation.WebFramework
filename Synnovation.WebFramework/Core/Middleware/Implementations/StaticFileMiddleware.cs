namespace Synnovation.WebFramework.Core.Middleware.Implementations;

public class StaticFileMiddleware : MiddlewareBase
{
    private readonly string _rootPath;

    public StaticFileMiddleware()
    {
        // Resolve wwwroot to an absolute path based on the application's base directory
        _rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

        if (!Directory.Exists(_rootPath))
        {
            throw new DirectoryNotFoundException($"The wwwroot directory does not exist at path: {_rootPath}");
        }
    }

    public override async Task<HttpResponse> InvokeAsync(HttpRequest request,
        Func<HttpRequest, Task<HttpResponse>> next)
    {
        // Normalize the path and map it to the wwwroot directory
        var filePath = Path.Combine(_rootPath, request.Path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(filePath))
        {
            Console.WriteLine($"[{DateTime.Now}] Serving File: {filePath}");
            var contentType = GetContentType(filePath);
            var content = await File.ReadAllTextAsync(filePath);
            return new HttpResponse(200, content) { ContentType = contentType };
        }

        Console.WriteLine($"[{DateTime.Now}] File Not Found: {filePath}");
        // Pass the request to the next middleware if the file is not found
        return await next(request);
    }

    private static string GetContentType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".html" => "text/html",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
    }
}