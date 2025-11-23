namespace EPTools.Core.Models;

public class DiscordWebHookMessage
{
    public string Content { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DiscordWebhookMessageEmbed Embed { get; set; } =  new();
}

public class DiscordWebhookMessageEmbed
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}