using MelonLoader;

namespace Poppers_server_notifier
{
    public static class Config
    {
        public static MelonPreferences_Category Category;

        public static MelonPreferences_Entry<bool> Notify;
        public static MelonPreferences_Entry<string> Webhook;

        public static void Initialize()
        {
            Category = MelonPreferences.CreateCategory("SNCFG");

            Notify = Category.CreateEntry(
                "Enable webhook notifications",
                false);

            Webhook = Category.CreateEntry(
                "Webhook URL",
                "https://discord.com/api/webhooks/your_webhook_url_here");

            MelonPreferences.Save();
        }

        public static void Save()
        {
            MelonPreferences.Save();
        }
    }
}