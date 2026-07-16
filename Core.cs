using MelonLoader;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using UnityEngine;

[assembly: MelonInfo(typeof(Poppers_server_notifier.Core), "Poppers server notifier", "1.0.0", "Popper", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace Poppers_server_notifier
{
    public class Core : MelonMod
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

        // Melon Stuff
        private MelonPreferences_Category SNCFG;
        private MelonPreferences_Entry<bool> Notify;
        private MelonPreferences_Entry<string> Webhook;
        private MelonPreferences_Entry<string> ServerName;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("I LIVE.");

            SNCFG = MelonPreferences.CreateCategory("SNCFG");

            Notify = SNCFG.CreateEntry<bool>("Enable webhook notifications", false);
            Webhook = SNCFG.CreateEntry<string>("Webhook URL", "https://discord.com/api/webhooks/your_webhook_url_here");
            ServerName = SNCFG.CreateEntry<string>("Server Name", "My Server");

            MelonPreferences.Save();

            MelonEvents.OnGUI.Subscribe(DrawMenu, 100);
        }

        private void DrawMenu()
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

            ServerName.Value = GUI.TextField(new Rect(Padding, y, window.width - Padding * 2, TextHeight), ServerName.Value);
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

        private void SendServerNotifications() 
        {
            if (Notify.Value)
            {
                DiscordEmbed embed = new DiscordEmbed
                {
                    title = "Server Notification",
                    description = $"Server Name: {ServerName.Value}\nStatus: Online",
                    color = 0x00FF00, // Green
                    footer = new DiscordFooter { text = "Poppers Server Notifier" },
                    timestamp = DateTime.UtcNow.ToString("o")
                };
                WebHookSender.SendEmbed(Webhook.Value, embed);
            }
        }

        public class DiscordEmbed
        {
            public string title;
            public string description;
            public int color;
            public DiscordFooter footer;
            public string timestamp;
        }

        public class DiscordFooter
        {
            public string text;
        }

        public static class WebHookSender
        {
            private static bool IsWebhookValid(string webhook)
            {
                return !string.IsNullOrWhiteSpace(webhook)
                    && webhook.StartsWith(
                        "https://discord.com/api/webhooks/",
                        StringComparison.OrdinalIgnoreCase);
            }

            private static void SendJson(string webhook, string json)
            {
                if (!IsWebhookValid(webhook))
                {
                    MelonLogger.Warning("Webhook URL is invalid.");
                    return;
                }

                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var request = (HttpWebRequest)WebRequest.Create(webhook);
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.NoContent)
                        {
                            throw new Exception($"Discord returned {(int)response.StatusCode}");
                        }
                    }
                }
                catch (WebException ex)
                {
                    MelonLogger.Error($"Webhook request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Error(ex.ToString());
                }
            }

            public static void SendMessage(string webhook, string message)
            {
                var payload = new
                {
                    content = message
                };

                SendJson(webhook, JsonConvert.SerializeObject(payload));
            }

            public static void SendEmbed(string webhook, DiscordEmbed embed)
            {
                var payload = new
                {
                    embeds = new[] { embed }
                };

                SendJson(webhook, JsonConvert.SerializeObject(payload));
            }
        }
    }
}