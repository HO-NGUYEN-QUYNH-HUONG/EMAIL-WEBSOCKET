using MailKit.Net.Smtp;
using MimeKit;
using MailWebSocketApp.Models;

namespace MailWebSocketApp;

public class MailHandler
{
    public static async Task SendMailAsync(MailData data)
    {
        Console.WriteLine($"[DEBUG] FROM: '{data.FromEmail}'");
        Console.WriteLine($"[DEBUG] TO: '{data.To}'"); // Dòng cần thêm
        Console.WriteLine($"[DEBUG] SUBJECT: '{data.Subject}'");
        Console.WriteLine($"[DEBUG] BODY: '{data.Body}'");

        if (string.IsNullOrWhiteSpace(data.To))
            throw new Exception("Không tìm thấy địa chỉ người nhận (To).");

        if (!MailboxAddress.TryParse(data.To, out var toAddress))
            throw new Exception("Địa chỉ người nhận không hợp lệ.");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender", data.FromEmail));

        message.To.Add(toAddress);
        message.Subject = data.Subject;
        message.Body = new TextPart("plain") { Text = data.Body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(data.FromEmail, data.AppPassword);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}


