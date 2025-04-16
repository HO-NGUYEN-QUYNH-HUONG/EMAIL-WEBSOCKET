using System.Text.Json.Serialization;

namespace MailWebSocketApp.Models;

public class MailData
{
    [JsonPropertyName("fromEmail")]
    public string FromEmail { get; set; } = "";

    [JsonPropertyName("appPassword")]
    public string AppPassword { get; set; } = "";

    [JsonPropertyName("to")]
    public string To { get; set; } = "";

    [JsonPropertyName("subject")]
    public string Subject { get; set; } = "";

    [JsonPropertyName("body")]
    public string Body { get; set; } = "";
}

