using Alta.Api.DataTransferModels.Models.Responses;
using MelonLoader;
using UnityEngine;

namespace Poppers_server_notifier.Modstuff
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

        // Colors for borders and accents matching your theme
        private static readonly Color borderColor = new Color(0.16f, 0.18f, 0.24f, 0.9f);

        public static void Draw()
        {
            GUIStyles.Initialize();

            // Use GUI.Window with GUIStyle.none instead of GUILayout.Window
            window = GUI.Window(1234, window, DrawWindow, GUIContent.none, GUIStyle.none);
        }

        private static void DrawWindow(int id)
        {
            // 1. Draw your custom dark background texture to fill the entire window area
            Rect windowRect = new Rect(0, 0, window.width, window.height);
            if (GUIStyles.BgTex != null)
            {
                GUI.DrawTexture(windowRect, GUIStyles.BgTex);
            }

            // 2. Draw 1px Outer Border
            DrawRectBorder(windowRect, borderColor, 1f);

            // 3. Draw Cyan Top Accent Strip (3 pixels tall)
            if (GUIStyles.AccentTex != null)
            {
                GUI.DrawTexture(new Rect(0, 0, window.width, 3f), GUIStyles.AccentTex);
            }

            GameServerInfo info = GameModeManager.CurrentGameServerInfo;
            string serverName = info?.Name ?? "Not Hosting";

            float y = HeaderHeight + Padding;

            void Next(float h) => y += h + RowSpacing;

            GUI.Box(new Rect(0, 0, window.width, HeaderHeight), "POPPERS DISCORD SERVER NOTIFIER", GUIStyles.Header);

            GUI.Label(new Rect(Padding, y, 100, LabelHeight), "Server Name:", GUIStyles.KeyStyle);
            Next(LabelHeight);

            GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, TextHeight), serverName, GUIStyles.readOnlyFieldStyle);
            Next(TextHeight);

            GUI.Label(new Rect(Padding, y, 100, LabelHeight), "Webhook:", GUIStyles.KeyStyle);
            Next(LabelHeight);

            Config.Webhook.Value = GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, TextHeight), Config.Webhook.Value, GUIStyles.readOnlyFieldStyle);
            Next(TextHeight);

            Config.Notify.Value = GUI.Toggle(new Rect(Padding, y, 180, 20), Config.Notify.Value, " Enable Notifications", GUIStyles.KeyStyle);
            Next(20);

            float buttonWidth = (window.width - Padding * 3) / 2;

            if (GUI.Button(new Rect(Padding, y, buttonWidth, ButtonHeight), "Test Webhook", GUIStyles.ButtonStyle))
            {
                WebHookSender.SendMessage(
                    Config.Webhook.Value,
                    testMessage);
            }

            if (GUI.Button(new Rect(Padding * 2 + buttonWidth, y, buttonWidth, ButtonHeight), "Save", GUIStyles.ButtonStyle))
            {
                Config.Save();
                MelonLogger.Msg("Preferences saved.");
            }

            GUI.DragWindow(new Rect(0, 0, window.width, HeaderHeight));
        }

        private static void DrawRectBorder(Rect r, Color c, float thickness)
        {
            GUI.DrawTexture(new Rect(r.x, r.y, r.width, thickness), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, c, 0f, 0f);
            GUI.DrawTexture(new Rect(r.x, r.yMax - thickness, r.width, thickness), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, c, 0f, 0f);
            GUI.DrawTexture(new Rect(r.x, r.y, thickness, r.height), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, c, 0f, 0f);
            GUI.DrawTexture(new Rect(r.xMax - thickness, r.y, thickness, r.height), Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, c, 0f, 0f);
        }
    }
}