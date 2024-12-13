using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Middleware.Implementations;
using Xunit;

namespace Synnovation.WebFramework.Tests;

public class AuthenticationMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_UnauthorizedRequest_Returns401()
    {
        // Arrange
        var unauthorizedRequest = new HttpRequest
        {
            Method = "GET",
            Path = "/test",
            Headers = new Dictionary<string, string>()
        };

        var middleware = new AuthenticationMiddleware();

        // Act
        var response = await middleware.InvokeAsync(
            unauthorizedRequest,
            request => Task.FromResult(new HttpResponse(200, "Success"))
        );

        // Assert
        Assert.Equal(401, response.StatusCode);
        Assert.Equal("Unauthorized", response.Body);
    }

    [Fact]
    public async Task InvokeAsync_AuthorizedRequest_Returns200()
    {
        // Arrange
        var authorizedRequest = new HttpRequest
        {
            Method = "GET",
            Path = "/test",
            Headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer token123" }
            }
        };

        var middleware = new AuthenticationMiddleware();

        // Act
        var response = await middleware.InvokeAsync(
            authorizedRequest,
            request => Task.FromResult(new HttpResponse(200, "Success"))
        );

        // Assert
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Success", response.Body);
        Assert.True(response.Headers.ContainsKey("X-Auth-Processed"));
        Assert.Equal("True", response.Headers["X-Auth-Processed"]);
    }
}