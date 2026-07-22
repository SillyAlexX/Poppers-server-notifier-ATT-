using System;
using System.IO;
using System.Net;
using System.Text;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Poppers_server_notifier.Modstuff
{
    public static class WebHookSender
    {
        public const string DefaultUsername = "Poppers Server Notifier";
        public const string DefaultAvatar = "https://raw.githubusercontent.com/SillyAlexX/Poppers-server-notifier-ATT-/refs/heads/master/Images/Screenshot%202025-01-28%20213952.png";

        private static bool IsWebhookValid(string webhook)
        {
            return !string.IsNullOrWhiteSpace(webhook) && webhook.StartsWith("https://discord.com/api/webhooks/", StringComparison.OrdinalIgnoreCase);
        }

        private static string SendJson(string webhook, string json, string method = "POST", bool expectResponse = false)
        {
            if (!IsWebhookValid(webhook))
            {
                MelonLogger.Warning("Webhook URL is invalid.");
                return null;
            }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = (HttpWebRequest)WebRequest.Create(webhook);
                request.Method = method;
                request.ContentType = "application/json";

                byte[] bytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = bytes.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (expectResponse)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseBody = reader.ReadToEnd();
                            // Parse message ID from Discord's JSON response payload
                            var jsonObject = JObject.Parse(responseBody);
                            return jsonObject["id"]?.ToString();
                        }
                    }
                    else
                    {
                        if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                            throw new Exception($"Discord returned {(int)response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
            }

            return null;
        }

        public static void SendMessage(string webhook, string message)
        {
            var payload = new
            {
                username = DefaultUsername,
                avatar_url = DefaultAvatar,
                content = message
            };

            SendJson(webhook, JsonConvert.SerializeObject(payload), "POST", false);
        }

        public static void SendEmbed(string webhook, DiscordEmbed embed)
        {
            var payload = new
            {
                username = DefaultUsername,
                avatar_url = DefaultAvatar,
                embeds = new[] { embed }
            };

            SendJson(webhook, JsonConvert.SerializeObject(payload), "POST", false);
        }

        public static string SendEmbedAndReturnId(string webhook, DiscordEmbed embed)
        {
            var payload = new
            {
                username = DefaultUsername,
                avatar_url = DefaultAvatar,
                embeds = new[] { embed }
            };

            string urlWithWait = webhook.Contains("?") ? $"{webhook}&wait=true" : $"{webhook}?wait=true";
            return SendJson(urlWithWait, JsonConvert.SerializeObject(payload), "POST", true);
        }

        public static void EditEmbed(string webhook, string messageId, DiscordEmbed embed)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                SendEmbed(webhook, embed);
                return;
            }

            var payload = new
            {
                embeds = new[] { embed }
            };

            string editUrl = $"{webhook.TrimEnd('/')}/messages/{messageId}";
            SendJson(editUrl, JsonConvert.SerializeObject(payload), "PATCH", false);
        }

        public static void DeleteMessage(string webhook, string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
                return;

            string deleteUrl = $"{webhook.TrimEnd('/')}/messages/{messageId}";
            SendJson(deleteUrl, "", "DELETE", false);
        }
    }
}