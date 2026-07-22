using HarmonyLib;
using MelonLoader;
using MelonLoader.Logging;

namespace Poppers_server_notifier.Modstuff
{
    internal class HostDetection
    {
        private static bool _hasTriggered = false;

        [HarmonyPatch(typeof(ServerHostingGameMode), nameof(ServerHostingGameMode.OnStartSucceeded))]
        public class ServerStartedPatch
        {
            static void Postfix()
            {
                if (_hasTriggered || ServerNotifier._serverNotified)
                    return;

                _hasTriggered = true;

                MelonLogger.Msg(ColorARGB.Chartreuse, "Server is online pinging discord server");
                ServerNotifier.SendServerNotification();
            }
        }
    }
}