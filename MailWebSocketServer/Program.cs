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

                Console.WriteLine($"[RAW RECEIVED]: {receivedText}");

                if (receivedText.StartsWith("send:"))
                {
                    var json = receivedText.Substring(5);
                    Console.WriteLine($"[JSON DATA]: {json}");

                    MailData? data = null;
                    string response;

                    try
                    {
                        data = JsonSerializer.Deserialize<MailData>(json);

                        if (data != null)
                        {
                            // Debug dữ liệu nhận được
                            Console.WriteLine($"[DEBUG] FROM: '{data.FromEmail}'");
                            Console.WriteLine($"[DEBUG] TO: '{data.To}'");
                            Console.WriteLine($"[DEBUG] SUBJECT: '{data.Subject}'");
                            Console.WriteLine($"[DEBUG] BODY: '{data.Body}'");

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
