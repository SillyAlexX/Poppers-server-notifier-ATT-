using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Poppers_server_notifier
{
    internal class MENU
    {
        // Menu Stuff
        private Rect window = new Rect(20, 20, 310, 310);

        private GUIStyle windowStyle;
        private GUIStyle headerStyle;
        private GUIStyle tabStyle;
        private GUIStyle activeTabStyle;
        private const float Padding = 10f;
        private const float RowSpacing = 8f;
        private const float LabelHeight = 20f;
        private const float TextHeight = 22f;
        private const float ToggleHeight = 20f;
        private const float ButtonHeight = 30f;
        private const float HeaderHeight = 35f;

        private string testMessage = "Test message from Poppers Discord Server Notifier mod.";

        public void DrawMenu()
        {
            SetupStyles();
            window = GUI.Window(1234, window, DrawWindow, " ", windowStyle);
        }

        private void DrawWindow(int id)
        {
            float y = HeaderHeight + Padding;

            void Next(float height)
            {
                y += height + RowSpacing;
            }

            GUI.Box(new Rect(0, 0, window.width, HeaderHeight), "POPPERS DISCORD SERVER NOTIFIER", headerStyle);

            GUI.Label(new Rect(Padding, y, 100, LabelHeight), "Server Name:");
            Next(LabelHeight);

            Core.ServerName.value = GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, TextHeight), ServerName.Value);
            Next(TextHeight);

            GUI.Label(new Rect(Padding, y, 100, 20), "Webhook:");
            Next(LabelHeight);

            Webhook.Value = GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, 22), Webhook.Value);
            Next(TextHeight);

            Notify.Value = GUI.Toggle(new Rect(Padding, y, 180, 20), Notify.Value, "Enable Notifications");
            Next(20);

            float buttonWidth = (window.width - Padding * 3) / 2;

            if (GUI.Button(new Rect(Padding, y, buttonWidth, ButtonHeight), "Test Webhook"))
            {
                WebHookSender.SendMessage(Webhook.Value, testMessage);
            }

            if (GUI.Button(new Rect(Padding * 2 + buttonWidth, y, buttonWidth, ButtonHeight), "Save"))
            {
                MelonPreferences.Save();
                MelonLogger.Msg("Preferences saved.");
            }

            GUI.DragWindow(new Rect(0, 0, window.width, HeaderHeight));
        }

        private void SetupStyles()
        {
            if (windowStyle != null)
                return;

            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.normal.background = MakeTexture(new Color(0.0f, 0.0f, 0.0f, 0.70f));
            windowStyle.normal.textColor = Color.clear;
            windowStyle.onNormal.textColor = Color.clear;

            headerStyle = new GUIStyle(GUI.skin.box);
            headerStyle.normal.background = MakeTexture(new Color(0.345f, 0.396f, 0.949f)); // Blurple
            headerStyle.normal.textColor = Color.white;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.alignment = TextAnchor.MiddleCenter;

            tabStyle = new GUIStyle(GUI.skin.button);
            tabStyle.normal.background = MakeTexture(new Color(0.25f, 0.26f, 0.29f));
            tabStyle.normal.textColor = Color.white;

            activeTabStyle = new GUIStyle(tabStyle);
            activeTabStyle.normal.background = MakeTexture(new Color(0.345f, 0.396f, 0.949f));
            activeTabStyle.normal.textColor = Color.white;
        }



        private Texture2D MakeTexture(Color color)
        {
            Texture2D tex = new Texture2D(1, 1);

            tex.SetPixel(0, 0, color);
            tex.Apply();

            return tex;
        }
    }
}
