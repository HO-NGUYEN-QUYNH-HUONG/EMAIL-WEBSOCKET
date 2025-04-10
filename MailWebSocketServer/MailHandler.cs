using MailKit.Net.Smtp;
using MimeKit;
using MailWebSocketApp.Models;

namespace MailWebSocketApp;

public class MailHandler
{
    public static async Task SendMailAsync(MailData data)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender", data.FromEmail));
        message.To.Add(MailboxAddress.Parse(data.To));
        message.Subject = data.Subject;
        message.Body = new TextPart("plain") { Text = data.Body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(data.FromEmail, data.AppPassword);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
