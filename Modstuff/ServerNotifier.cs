using MelonLoader;
using MelonLoader.Utils;
using Poppers_server_notifier.Modstuff;

namespace Poppers_server_notifier
{
    internal class ServerNotifier
    {
        public static bool _serverNotified = false;
        private static readonly string MessageIdFilePath = Path.Combine(MelonEnvironment.UserDataDirectory, "PoppersServerNotifier_LastMessageId.txt");

        public static void SendServerNotification()
        {
            if (!Config.Notify.Value)
                return;

            int playerCount = PlayerUpdates.OnlinePlayers.Count;
            string playerListText = playerCount > 0 ? string.Join(", ", PlayerUpdates.OnlinePlayers) : "No players online";

            // Clear out any old message from a previous session first if it exists
            DeletePreviousMessage();

            var embed = CreatePlayerEmbed("Server is up!", playerCount, playerListText);

            // Send a brand new message and save its ID
            string newMsgId = WebHookSender.SendEmbedAndReturnId(Config.Webhook.Value, embed);
            if (!string.IsNullOrEmpty(newMsgId))
            {
                SaveLastMessageId(newMsgId);
            }

            MelonLogger.Msg("Server initial notification sent.");
            _serverNotified = true;
        }

        public static void UpdateServerEmbed(string statusMessage)
        {
            int playerCount = PlayerUpdates.OnlinePlayers.Count;
            string playerListText = playerCount > 0 ? string.Join(", ", PlayerUpdates.OnlinePlayers) : "No players online";

            if (!Config.Notify.Value)
                return;

            // 1. Delete the old notification message from Discord
            DeletePreviousMessage();

            // 2. Pass both the status message and the player list text here!
            var embed = CreatePlayerEmbed(statusMessage, playerCount, playerListText);
            string newMsgId = WebHookSender.SendEmbedAndReturnId(Config.Webhook.Value, embed);

            // 3. Save the new message ID for the next update/leave event
            if (!string.IsNullOrEmpty(newMsgId))
            {
                SaveLastMessageId(newMsgId);
            }

            MelonLogger.Msg("Refreshed Discord embed with latest player update.");
        }

        private static void DeletePreviousMessage()
        {
            try
            {
                if (File.Exists(MessageIdFilePath))
                {
                    string lastId = File.ReadAllText(MessageIdFilePath).Trim();
                    if (!string.IsNullOrEmpty(lastId))
                    {
                        WebHookSender.DeleteMessage(Config.Webhook.Value, lastId);
                    }
                    File.Delete(MessageIdFilePath); // Clear the file after deleting
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to delete previous message: {ex.Message}");
            }
        }

        private static void SaveLastMessageId(string messageId)
        {
            try
            {
                File.WriteAllText(MessageIdFilePath, messageId);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to save message ID: {ex.Message}");
            }
        }

        private static DiscordEmbed CreatePlayerEmbed(string headerText, int playerCount, string playerListString)
        {
            return new DiscordEmbed
            {
                title = "Server Online!",
                description = $"**{headerText}**\n\n**Players Online ({playerCount}):**\n{playerListString}",
                color = 0x57F287,
                footer = new DiscordFooter
                {
                    text = "Poppers Server Notifier"
                },
                timestamp = DateTime.UtcNow.ToString("o")
            };
        }
    }
}