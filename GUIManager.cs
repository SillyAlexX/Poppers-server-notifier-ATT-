using MelonLoader;
using UnityEngine;

namespace Poppers_server_notifier
{
    public static class GUIManager
    {
        private static Rect window = new Rect(20, 20, 310, 310);

        const float Padding = 10;
        const float RowSpacing = 8;
        const float LabelHeight = 20;
        const float TextHeight = 22;
        const float ButtonHeight = 30;
        const float HeaderHeight = 35;

        private static readonly string testMessage = "Test message from Poppers Discord Server Notifier mod.";

        public static void Draw()
        {
            GUIStyles.Initialize();
            window = GUI.Window(1234, window, DrawWindow, "", GUIStyles.Window);
        }

        private static void DrawWindow(int id)
        {
            float y = HeaderHeight + Padding;

            void Next(float h) => y += h + RowSpacing;

            GUI.Box(new Rect(0, 0, window.width, HeaderHeight), "POPPERS DISCORD SERVER NOTIFIER", GUIStyles.Header);

            GUI.Label(new Rect(Padding, y, 100, LabelHeight), "Server Name:");
            Next(LabelHeight);

            GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, TextHeight), GameModeManager.CurrentGameServerInfo.Name, GUIStyles.readOnlyFieldStyle);
            Next(TextHeight);

            GUI.Label(new Rect(Padding, y, 100, LabelHeight), "Webhook:");
            Next(LabelHeight);

            Config.Webhook.Value = GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, TextHeight), Config.Webhook.Value);
            Next(TextHeight);

            Config.Notify.Value = GUI.Toggle(new Rect(Padding, y, 180, 20), Config.Notify.Value, "Enable Notifications");
            Next(20);

            float buttonWidth = (window.width - Padding * 3) / 2;

            if (GUI.Button(new Rect(Padding, y, buttonWidth, ButtonHeight), "Test Webhook"))
            {
                WebHookSender.SendMessage(
                    Config.Webhook.Value,
                    testMessage);
            }

            if (GUI.Button(new Rect(Padding * 2 + buttonWidth, y, buttonWidth, ButtonHeight), "Save"))
            {
                Config.Save();
                MelonLogger.Msg("Preferences saved.");
            }

            GUI.DragWindow(new Rect(0, 0, window.width, HeaderHeight));
        }
    }
}