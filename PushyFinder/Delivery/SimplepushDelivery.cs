using System.Collections.Generic;
using System.Threading.Tasks;
using Dalamud.Utility;
using Flurl.Http;

namespace PushyFinder.Delivery;

public class SimplepushDelivery : IDelivery
{
    public static readonly string SimplepushApi = "https://api.simplepu.sh/v1/notifications/json";

    public bool IsActive => !Plugin.Configuration.SimplepushApiToken.IsNullOrWhitespace();

    public void Deliver(string title, string text)
    {
        Task.Run(() => DeliverAsync(title, text));
    }

    private static async Task DeliverAsync(string title, string text)
    {
        var notificationTitle = Plugin.Configuration.SimplepushTitle.IsNullOrWhitespace()
            ? title
            : Plugin.Configuration.SimplepushTitle;

        var args = new Dictionary<string, string>
        {
            { "title", notificationTitle },
            { "content", text }
        };

        if (!Plugin.Configuration.SimplepushTopic.IsNullOrWhitespace())
            args.Add("topic", Plugin.Configuration.SimplepushTopic);

        if (!Plugin.Configuration.SimplepushTopicAuthToken.IsNullOrWhitespace())
            args.Add("topicAuthToken", Plugin.Configuration.SimplepushTopicAuthToken);

        try
        {
            await SimplepushApi
                .WithHeader("API-Token", Plugin.Configuration.SimplepushApiToken)
                .PostJsonAsync(args);
            Service.PluginLog.Debug("Sent Simplepush message");
        }
        catch (FlurlHttpException e)
        {
            Service.PluginLog.Error($"Failed to make Simplepush request: '{e.Message}'");
            Service.PluginLog.Error($"{e.StackTrace}");
        }
    }
}
