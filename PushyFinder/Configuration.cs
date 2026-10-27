using System;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace PushyFinder;

[Serializable]
public class Configuration : IPluginConfiguration
{
    // the below exist just to make saving less cumbersome
    [NonSerialized]
    private IDalamudPluginInterface? PluginInterface;

    public string PushoverAppKey { get; set; } = "";
    public string PushoverUserKey { get; set; } = "";
    public string PushoverDevice { get; set; } = "";
    public string DiscordWebhookToken { get; set; } = "";
    public string DiscordMessage { get; set; } = "";
    public string NtfyServer { get; set; } = "https://ntfy.sh/";
    public string NtfyTopic { get; set; } = "";
    public string NtfyToken { get; set; } = "";
    public string SimplepushApiToken { get; set; } = "";
    public string SimplepushTopic { get; set; } = "";
    public string SimplepushTopicAuthToken { get; set; } = "";
    public string SimplepushTitle { get; set; } = "";
    // Retained so existing plugin configs still deserialize after the 2026 API rebuild.
    public string SimplepushKey { get; set; } = "";
    public bool EnableForDutyPops { get; set; } = true;
    public bool IgnoreAfkStatus { get; set; } = false;
    public bool DiscordUseEmbed { get; set; } = true;
    public uint DiscordEmbedColor { get; set; } = 0x00FF00;
    public int Version { get; set; } = 1;

    public void Initialize(IDalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;
        Migrate();
    }

    private void Migrate()
    {
        if (Version >= 2)
            return;

        // Old Simplepush keys were migrated to topics by the app.
        if (string.IsNullOrWhiteSpace(SimplepushTopic) && !string.IsNullOrWhiteSpace(SimplepushKey))
            SimplepushTopic = SimplepushKey;

        Version = 2;
    }

    public void Save()
    {
        PluginInterface!.SavePluginConfig(this);
    }
}
