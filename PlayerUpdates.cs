using MelonLoader;
using MelonLoader.Logging;

namespace Poppers_server_notifier
{
    internal class PlayerUpdates
    {
        public static string ActiveMessageId { get; set; } = null;

        public static void OnPlayerJoined(Player player)
        {
            string username = player?.UserInfo?.Username ?? "Unknown";
            MelonLogger.Msg(ColorARGB.Chartreuse, $"Player joined: {username}");

            UpdateServerEmbed();
        }

        public static void OnPlayerLeft(Player player)
        {
            string username = player?.UserInfo?.Username ?? "Unknown";
            MelonLogger.Msg(ColorARGB.Chartreuse, $"Player left: {username}");

            UpdateServerEmbed();
        }

        private static void UpdateServerEmbed()
        {
            if (!Config.Notify.Value)
                return;

            // Fetch current online players
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

            // If we already sent a message, edit it. Otherwise, send a new one and save its ID.
            if (!string.IsNullOrEmpty(ActiveMessageId))
            {
                WebHookSender.EditEmbed(Config.Webhook.Value, ActiveMessageId, embed);
                MelonLogger.Msg("Updated existing Discord embed with new player list.");
            }
            else
            {
                ActiveMessageId = WebHookSender.SendEmbedAndReturnId(Config.Webhook.Value, embed);
                MelonLogger.Msg("Sent initial Discord embed.");
            }
        }
    }
}