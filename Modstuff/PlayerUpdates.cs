using MelonLoader;

namespace Poppers_server_notifier.Modstuff
{
    internal class PlayerUpdates
    {
        public static void OnPlayerJoined(Player player)
        {
            string username = player?.UserInfo?.Username ?? "Unknown";
            MelonLogger.Msg($"Player joined: {username}");

            ServerNotifier.UpdateServerEmbed($"{username} joined the server.");
        }

        public static void OnPlayerLeft(Player player)
        {
            string username = player?.UserInfo?.Username ?? "Unknown";
            MelonLogger.Msg($"Player left: {username}");

            ServerNotifier.UpdateServerEmbed($"{username} left the server.");
        }
    }
}