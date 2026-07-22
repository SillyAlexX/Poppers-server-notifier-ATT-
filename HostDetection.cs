using HarmonyLib;
using MelonLoader;
using MelonLoader.Logging;

namespace Poppers_server_notifier
{
    internal class HostDetection
    {

        [HarmonyPatch(typeof(ServerHostingGameMode), nameof(ServerHostingGameMode.OnStartSucceeded))]
        public class ServerStartedPatch
        {
            static void Postfix()
            { 
                MelonLogger.Msg(ColorARGB.Chartreuse, "Server is online pinging discord server");
                ServerNotifier.SendServerNotification();
            }
        }
    }
}
