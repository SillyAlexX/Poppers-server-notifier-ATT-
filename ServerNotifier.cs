using MelonLoader;
using Alta.Api.DataTransferModels.Models.Responses;
using System.Linq;

namespace Poppers_server_notifier
{
    internal class ServerNotifier
    {
        public static bool _serverNotified = false;

        public static void SendServerNotification()
        {
            if (_serverNotified)
                return;

            if (!Config.Notify.Value)
                return;

            // Get the usernames from the PlayerList static property
            var playerNames = PlayerList.LastList?
                .Select(p => p.UserInfo?.Username)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToArray();

            string playerListString = (playerNames != null && playerNames.Length > 0)
                ? string.Join(", ", playerNames)
                : "No players online";

            var embed = new DiscordEmbed
            {
                title = "Server Online!",
                description = $"**Server is up!**\n\n**Players Online ({playerNames?.Length ?? 0}):**\n{playerListString}",
                color = 0x57F287,
                footer = new DiscordFooter
                {
                    text = "Poppers Server Notifier"
                },
                timestamp = DateTime.UtcNow.ToString("o")
            };

            WebHookSender.SendEmbed(Config.Webhook.Value, embed);
            MelonLogger.Msg("Server notification sent.");
            _serverNotified = true;
        }
    }
}
