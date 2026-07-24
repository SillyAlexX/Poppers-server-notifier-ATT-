using System.Collections.Generic;
using MelonLoader;

namespace Poppers_server_notifier.Modstuff
{
    internal class PlayerUpdates
    {
        // 1. Keep track of online players in a list
        public static List<string> OnlinePlayers = new List<string>();

        public static void OnPlayerJoined(Player player)
        {
            string username = player?.UserInfo?.Username ?? "Unknown";
            MelonLogger.Msg($"Player joined: {username}");

            // 2. Add the player to the list if they aren't already in it
            if (!OnlinePlayers.Contains(username))
            {
                OnlinePlayers.Add(username);
            }

            // 3. Now send the update (your embed method can read PlayerUpdates.OnlinePlayers)
            ServerNotifier.UpdateServerEmbed($"{username} joined the server.");
        }

        public static void OnPlayerLeft(Player player)
        {
            string username = player?.UserInfo?.Username ?? "Unknown";
            MelonLogger.Msg($"Player left: {username}");

            // 4. Remove the player from the list when they leave
            if (OnlinePlayers.Contains(username))
            {
                OnlinePlayers.Remove(username);
            }

            ServerNotifier.UpdateServerEmbed($"{username} left the server.");
        }
    }
}