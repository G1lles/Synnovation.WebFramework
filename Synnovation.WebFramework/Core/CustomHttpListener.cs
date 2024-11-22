using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Synnovation.WebFramework.Core;

public class CustomHttpListener
{
    private readonly int _port;
    private readonly TcpListener _listener;

    public CustomHttpListener(int port)
    {
        _port = port;
        _listener = new TcpListener(IPAddress.Any, _port);
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"Listening on port {_port}...");
    }

    public async Task ListenAsync(Func<HttpRequest, Task<HttpResponse>> requestHandler)
    {
        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();
            _ = Task.Run(async () => await HandleClientAsync(client, requestHandler));
        }
    }

    private async Task HandleClientAsync(TcpClient client, Func<HttpRequest, Task<HttpResponse>> requestHandler)
    {
        client.ReceiveTimeout = 5000;
        client.SendTimeout = 5000;

        try
        {
            await using var stream = client.GetStream();
            var buffer = new byte[4096];
            var bytesRead = await stream.ReadAsync(buffer);
            if (bytesRead == 0)
            {
                Console.WriteLine("Received empty request.");
                return;
            }

            var requestString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            HttpRequest request;
            try
            {
                request = HttpRequest.Parse(requestString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing request: {ex.Message}");
                var errorResponse = new HttpResponse(400, "Bad Request");
                await SendResponseAsync(stream, errorResponse);
                return;
            }

            var response = await requestHandler(request);
            await SendResponseAsync(stream, response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    private async Task SendResponseAsync(NetworkStream stream, HttpResponse response)
    {
        var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
        await stream.WriteAsync(responseBytes);
    }

    public void Stop()
    {
        _listener.Stop();
    }
}