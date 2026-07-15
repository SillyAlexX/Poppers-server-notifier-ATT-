using MelonLoader;
using Mono.CSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: MelonInfo(typeof(Poppers_server_notifier.Core), "Poppers server notifier", "1.0.0", "Popper", null)]
[assembly: MelonGame("Alta", "A Township Tale")]

namespace Poppers_server_notifier
{
    public class Core : MelonMod
    {
        // Menu Stuff
        private Rect window = new Rect(20, 20, 310, 500);

        private GUIStyle windowStyle;
        private GUIStyle headerStyle;
        private GUIStyle tabStyle;
        private GUIStyle activeTabStyle;

        private int selectedTab = 0;

        // Melon Stuff
        private MelonPreferences_Category SNCFG;
        private MelonPreferences_Entry<bool> Notify;
        private MelonPreferences_Entry<string> Webhook;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("I LIVE.");

            SNCFG = MelonPreferences.CreateCategory("SNCFG");
            
            Notify = SNCFG.CreateEntry<bool>("Enable webhook notifications", true);
            Webhook = SNCFG.CreateEntry<string>("Webhook URL", "https://discord.com/api/webhooks/your_webhook_url_here");

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
            // Header
            GUI.Box(new Rect(0, 0, window.width, 35), "POPPERS DISCORD SERVER NOTIFYER", headerStyle);


            // Tabs
            string[] tabs ={"TEST WEBHOOK","SAVE"};

            float width = window.width / tabs.Length;

            for (int i = 0; i < tabs.Length; i++)
            {
                if (GUI.Button(new Rect(i * width,35,width,30),tabs[i], selectedTab == i ? activeTabStyle : tabStyle ))
                {
                    selectedTab = i;
                }
            }


            Rect content = new Rect(0, 65, window.width, window.height - 65);

            //MAIN
            GUI.Label( new Rect(10,80,200,20),"Not done yet" );


            GUI.DragWindow(new Rect( 0, 0, window.width, 35 ));
        }



        private void SetupStyles()
        {
            if (windowStyle != null)
                return;

            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.normal.background = MakeTexture(new Color(0.2f, 0.21f, 0.23f, 0.95f));
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