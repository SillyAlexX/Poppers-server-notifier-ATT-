using MelonLoader;
using Mono.CSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

[assembly: MelonInfo(typeof(Poppers_server_notifier.Core), "Poppers server notifier", "1.0.0", "Popper", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace Poppers_server_notifier
{
    public class Core : MelonMod
    {
        private MelonPreferences_Category SNCFG;
        private MelonPreferences_Entry<bool> Notify;
        private MelonPreferences_Entry<string> Webhook;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("I LIVE.");

            Directory.CreateDirectory("Mods/sncfg");

            SNCFG = MelonPreferences.CreateCategory("CFG");
            SNCFG.SetFilePath("Mods/sncfg/SN.cfg");

            Notify = SNCFG.CreateEntry<bool>("Enable webhook notifications", true);
            Webhook = SNCFG.CreateEntry<string>("Webhook URL", "https://discord.com/api/webhooks/your_webhook_url_here");

            MelonPreferences.Save();

            MelonEvents.OnGUI.Subscribe(DrawMenu, 100);
        }

        private void DrawMenu()
        {
            float width = 400;
            float height = 300;

            Rect window = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);

            GUI.Box(window, "Poppers Server Notifier Config");

            GUI.Label(new Rect(window.x + 20, window.y + 40, 220, 30), "Enable webhook notifications:");

            bool oldValue = Notify.Value;

            Notify.Value = GUI.Toggle(new Rect(window.x + 250, window.y + 40, 50, 30),Notify.Value, "");

            if (oldValue != Notify.Value)
            {
                MelonPreferences.Save();
            }
        }
    }

    public class WebHookSender
    {
        public static void Send(string webhook, string message)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var request = (HttpWebRequest)WebRequest.Create(webhook);
            request.Method = "POST";
            request.ContentType = "application/json";

            string json = "{\"content\":\"" + message.Replace("\"", "\\\"") + "\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // Discord returns HTTP 204 on success.
            }
        }
    }
}