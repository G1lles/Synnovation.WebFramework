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
        _listener = new TcpListener(System.Net.IPAddress.Any, _port);
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
            _ = Task.Run(() => HandleClientAsync(client, requestHandler));
        }
    }

    private async Task HandleClientAsync(TcpClient client, Func<HttpRequest, Task<HttpResponse>> requestHandler)
    {
        try
        {
            using var stream = client.GetStream();
            var buffer = new byte[4096];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var requestString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var request = HttpRequest.Parse(requestString);
            var response = await requestHandler(request);

            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
        finally
        {
            client.Close();
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}
