namespace MailWebSocketApp.Models;

public class MailData
{
    public string FromEmail { get; set; } = "";
    public string AppPassword { get; set; } = "";
    public string To { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";
}
