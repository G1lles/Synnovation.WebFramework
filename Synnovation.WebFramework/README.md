# Middleware System Documentation

## Overview

The middleware system in the **Synnovation WebFramework** allows developers to process HTTP requests and responses
through
a sequence of modular components, referred to as middleware. Middleware components can handle tasks such as logging,
static file serving, authentication (not implemented), and more.

## How Middleware Works

The middleware system uses a **pipeline** design pattern. Each middleware component can:

1. **Process Incoming Requests:** Perform actions on the HTTP request.
2. **Call the Next Middleware:** Pass control to the next middleware in the pipeline.
3. **Process Outgoing Responses:** Perform actions on the HTTP response returned by subsequent middleware.

### Core Components

1. **`MiddlewareBase`**
    - An abstract base class that all middleware components must inherit.
    - Defines the contract for middleware with the `InvokeAsync` method.

```csharp
public abstract class MiddlewareBase
{
    public MiddlewareBase? Next { get; set; }

    protected MiddlewareBase(MiddlewareBase? next = null)
    {
        Next = next;
    }

    public abstract Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next);
}
```

2. **`MiddlewarePipeline`**
    - Manages the sequence of middleware components.
    - Responsible for invoking middleware in the order they are added.

```csharp
public class MiddlewarePipeline
{
    private MiddlewareBase? _first;

    public MiddlewarePipeline Use(MiddlewareBase middleware)
    {
        if (_first == null)
        {
            _first = middleware;
        }
        else
        {
            var current = _first;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = middleware;
        }
        return this;
    }

    public async Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> finalHandler)
    {
        return _first != null ? await _first.InvokeAsync(request, finalHandler) : await finalHandler(request);
    }
}
```

3. **Middleware Implementations**
    - **`LoggingMiddleware`**: Logs incoming requests and outgoing responses.
    - **`StaticFileMiddleware`**: Serves static files from the `wwwroot` directory.

### Example Middleware: LoggingMiddleware

```csharp
public class LoggingMiddleware : MiddlewareBase
{
    public LoggingMiddleware(MiddlewareBase? next = null) : base(next) { }

    public override async Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next)
    {
        Console.WriteLine($"[{DateTime.Now}] Incoming request: {request.Method} {request.Path}");

        var response = await next(request);

        Console.WriteLine($"[{DateTime.Now}] Outgoing response: {response.StatusCode}");
        return response;
    }
}
```

---

## Adding Middleware to the Pipeline

To add middleware to the framework, follow these steps:

1. **Create a Middleware Class**
    - Inherit from `MiddlewareBase`.
    - Override the `InvokeAsync` method to define custom behavior.

```csharp
public class CustomMiddleware : MiddlewareBase
{
    public CustomMiddleware(MiddlewareBase? next = null) : base(next) { }

    public override async Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next)
    {
        // Pre-processing logic (e.g., authentication, validation)
        Console.WriteLine("Custom Middleware: Pre-processing request");

        var response = await next(request);

        // Post-processing logic (e.g., response modification)
        Console.WriteLine("Custom Middleware: Post-processing response");

        return response;
    }
}
```

2. **Register Middleware in the Pipeline**
    - Use the `MiddlewarePipeline.Use` method to add middleware in sequence.

```csharp
var pipeline = new MiddlewarePipeline();

pipeline
    .Use(new LoggingMiddleware())
    .Use(new StaticFileMiddleware())
    .Use(new CustomMiddleware());

var service = new HttpListenerService(prefixes, pipeline);
await service.RunAsync();
```

---

## Execution Order

Middleware is executed in the order it is added to the pipeline:

1. Incoming requests flow **down** the pipeline.
2. Outgoing responses flow **up** the pipeline.

For example:

```
Request ---> LoggingMiddleware ---> StaticFileMiddleware ---> CustomMiddleware ---> FinalHandler
Response <--- LoggingMiddleware <--- StaticFileMiddleware <--- CustomMiddleware <--- FinalHandler
```

---

## Benefits of the Middleware System

1. **Modularity:** Each middleware handles a specific responsibility.
2. **Reusability:** Middleware can be reused across different applications.
3. **Scalability:** Add or remove middleware without affecting the rest of the pipeline.
4. **Flexibility:** Developers can define custom behaviors at various stages of request and response handling.

---

## Summary

- Middleware is a powerful mechanism in the **Synnovation WebFramework** for processing HTTP requests and responses.
- Developers can leverage built-in middleware like `LoggingMiddleware` and `StaticFileMiddleware` or create custom
  middleware.
- Use the `MiddlewarePipeline` to compose and manage middleware components in the desired order.
