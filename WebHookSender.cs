using System;
using System.Net;
using System.Text;
using MelonLoader;
using Newtonsoft.Json;

namespace Poppers_server_notifier
{
    public static class WebHookSender
    {
        private static bool IsWebhookValid(string webhook)
        {
            return !string.IsNullOrWhiteSpace(webhook) &&
                   webhook.StartsWith(
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
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12;

                var request =
                    (HttpWebRequest)WebRequest.Create(webhook);

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
                        throw new Exception($"Discord returned {(int)response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
            }
        }

        public static void SendMessage(string webhook, string message)
        {
            SendJson(webhook,
                JsonConvert.SerializeObject(new
                {
                    content = message
                }));
        }

        public static void SendEmbed(string webhook, DiscordEmbed embed,
            string username = null, string avatarUrl = null)
        {
            var payload = new
            {
                username = username,
                avatar_url = avatarUrl,
                embeds = new[] { embed }
            };

            SendJson(webhook, JsonConvert.SerializeObject(payload));
        }
    }
}