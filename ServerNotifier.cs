using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace Poppers_server_notifier
{
    internal class ServerNotifier
    {

        public static void SendServerNotification()
        {
            if (!Config.Notify.Value)
                return;

            var embed = new DiscordEmbed
            {
                title = "Server Notification",
                description =
                    $"Server Name: {Config.ServerName.Value}\nStatus: Online",
                color = 0x00FF00,
                footer = new DiscordFooter
                {
                    text = "Poppers Server Notifier"
                },
                timestamp = DateTime.UtcNow.ToString("o")
            };

            WebHookSender.SendEmbed(Config.Webhook.Value, embed);

            if (GameModeManager.CurrentMode is ServerHostingGameMode)
            {
                MelonLogger.Msg("Server ping sent");
            }
        }
    }
}
