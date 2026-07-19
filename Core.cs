using MelonLoader;

[assembly: MelonInfo(typeof(Poppers_server_notifier.Core), "Poppers server notifier", "1.0.0", "Popper", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace Poppers_server_notifier
{
    public class Core : MelonMod
    {
        public static Core Instance;

        public override void OnInitializeMelon()
        {
            Instance = this;

            LoggerInstance.Msg("I LIVE.");

            Config.Initialize();

            MelonEvents.OnGUI.Subscribe(GUIManager.Draw, 100);
        }

        public override void OnDeinitializeMelon()
        {
            ServerNotifier._serverNotified = false;
        }
    }
}