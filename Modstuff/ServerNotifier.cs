using MelonLoader;
using MelonLoader.Logging;
using MelonLoader.Utils;

namespace Poppers_server_notifier.Modstuff
{
    internal class ServerNotifier
    {
        public static bool _serverNotified = false;
        private static readonly string MessageIdFilePath = Path.Combine(MelonEnvironment.UserDataDirectory, "PoppersServerNotifier_Messages.txt");

        public static void SendServerNotification()
        {
            if (_serverNotified)
                return;

            if (!Config.Notify.Value)
                return;

            var embed = CreatePlayerEmbed("Server is up!");

            string newMsgId = WebHookSender.SendEmbedAndReturnId(Config.Webhook.Value, embed);

            if (!string.IsNullOrEmpty(newMsgId))
            {
                SaveMessageId(newMsgId);
            }

            MelonLogger.Msg("Server initial notification sent.");
            _serverNotified = true;
        }

        public static void UpdateServerEmbed(string statusMessage)
        {
            if (!Config.Notify.Value)
                return;

            var embed = CreatePlayerEmbed(statusMessage);
            var messageIds = LoadMessageIds();

            if (messageIds.Count > 0)
            {
                string activeMessageId = messageIds[0];
                WebHookSender.EditEmbed(Config.Webhook.Value, activeMessageId, embed);

                for (int i = 1; i < messageIds.Count; i++)
                {
                    WebHookSender.DeleteMessage(Config.Webhook.Value, messageIds[i]);
                }

                SaveSingleMessageId(activeMessageId);
                MelonLogger.Msg(ColorARGB.Chartreuse,"Updated existing Discord embed and cleaned up extra messages.");
            }
            else
            {
                string newMsgId = WebHookSender.SendEmbedAndReturnId(Config.Webhook.Value, embed);
                if (!string.IsNullOrEmpty(newMsgId))
                {
                    SaveSingleMessageId(newMsgId);
                }
            }
        }

        private static DiscordEmbed CreatePlayerEmbed(string headerText)
        {
            var playerNames = PlayerList.LastList?
                .Select(p => p.UserInfo?.Username)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToArray();

            string playerListString = playerNames != null && playerNames.Length > 0
                ? string.Join(", ", playerNames)
                : "No players online";

            return new DiscordEmbed
            {
                title = "Server Online!",
                description = $"**{headerText}**\n\n**Players Online ({playerNames?.Length ?? 0}):**\n{playerListString}",
                color = 0x57F287,
                footer = new DiscordFooter
                {
                    text = "Poppers Server Notifier"
                },
                timestamp = DateTime.UtcNow.ToString("o")
            };
        }

        private static void SaveMessageId(string messageId)
        {
            try
            {
                File.AppendAllLines(MessageIdFilePath, new[] { messageId });
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to save message ID: {ex.Message}");
            }
        }

        private static void SaveSingleMessageId(string messageId)
        {
            try
            {
                File.WriteAllText(MessageIdFilePath, messageId + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to overwrite message ID file: {ex.Message}");
            }
        }

        private static List<string> LoadMessageIds()
        {
            try
            {
                if (File.Exists(MessageIdFilePath))
                {
                    return File.ReadAllLines(MessageIdFilePath)
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to load message IDs: {ex.Message}");
            }
            return new List<string>();
        }
    }
}