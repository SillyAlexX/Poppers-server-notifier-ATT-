using MelonLoader;
using Alta.Api.DataTransferModels.Models.Responses;

namespace Poppers_server_notifier
{
    internal class ServerNotifier
    {
        public static bool _serverNotified;

        public static void SendServerNotification()
        {
            if (_serverNotified)
                return;

            if (!Config.Notify.Value)
                return;

            if (GameModeManager.CurrentMode is not ServerHostingGameMode)
                return;

            GameServerInfo info = GameModeManager.CurrentGameServerInfo;

            if (info == null)
                return;

            string players = "No players online.";

            if (info.OnlinePlayers != null && info.OnlinePlayers.Any())
            {
                players = string.Join(
                    "\n",
                    info.OnlinePlayers.Select(p => $"• {p.Username}")
                );
            }

            var embed = new DiscordEmbed
            {
                title = "Server Online!",
                description = $@"Server: {info.Name} Players: {info.CurrentPlayerCount}/{info.PlayerLimit} {players}",
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
