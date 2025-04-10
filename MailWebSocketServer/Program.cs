using System.Net.WebSockets;
using System.Text;
using MailWebSocketApp;
using MailWebSocketApp.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();
app.UseDefaultFiles();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var receivedText = Encoding.UTF8.GetString(buffer, 0, result.Count);

                if (receivedText.StartsWith("send:"))
                {
                    var json = receivedText.Substring(5);
                    MailData? data = null;
                    string response;

                    try
                    {
                        data = JsonSerializer.Deserialize<MailData>(json);

                        if (data != null)
                        {
                            await MailHandler.SendMailAsync(data);
                            response = "Email sent successfully!";
                        }
                        else
                        {
                            response = "Invalid data received!";
                        }
                    }
                    catch (Exception ex)
                    {
                        response = $"Failed to send email: {ex.Message}";
                    }

                    await webSocket.SendAsync(
                        Encoding.UTF8.GetBytes(response),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                }
            }
        }
    }
    else
    {
        await next();
    }
});

app.Run();
