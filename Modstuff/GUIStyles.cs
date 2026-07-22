using UnityEngine;

namespace Poppers_server_notifier.Modstuff
{
    public static class GUIStyles
    {
        public static GUIStyle Window;
        public static GUIStyle Header;
        public static GUIStyle KeyStyle;
        public static GUIStyle ValueStyle;
        public static GUIStyle ButtonStyle;
        public static GUIStyle readOnlyFieldStyle;

        public static Texture2D AccentTex;
        public static Texture2D BgTex;

        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            // Textures
            BgTex = MakeTexture(new Color(0.05f, 0.05f, 0.07f, 0.94f));
            AccentTex = MakeTexture(new Color(0f, 0.9f, 1f, 1f)); // Cyan accent color

            var btnNormal = MakeTexture(new Color(0.08f, 0.09f, 0.12f, 0.8f)); // Dark/blackish button
            var btnHover = MakeTexture(new Color(0.12f, 0.15f, 0.2f, 0.9f));
            var btnActive = MakeTexture(new Color(0f, 0.9f, 1f, 0.25f));

            Window = new GUIStyle(GUI.skin.window)
            {
                normal = { background = BgTex, textColor = Color.clear },
                onNormal = { textColor = Color.clear },
                active = { background = BgTex, textColor = Color.clear },
                onActive = { background = BgTex, textColor = Color.clear },
                hover = { background = BgTex, textColor = Color.clear },
                onHover = { background = BgTex, textColor = Color.clear },
                border = new RectOffset(0, 0, 0, 0),
                overflow = new RectOffset(0, 0, 0, 0)
            };

            Header = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0f, 0.94f, 1f, 0.9f) },
                padding = new RectOffset(0, 0, 4, 4)
            };

            KeyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                normal = { textColor = new Color(0.54f, 0.58f, 0.65f) },
                padding = new RectOffset(10, 0, 2, 2)
            };

            ValueStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.9f, 0.93f, 0.96f) },
                padding = new RectOffset(0, 10, 2, 2)
            };

            // Black button with cyan text & outlined feel
            ButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { background = btnNormal, textColor = new Color(0f, 0.9f, 1f) },
                hover = { background = btnHover, textColor = Color.white },
                active = { background = btnActive, textColor = Color.white },
                margin = new RectOffset(8, 8, 4, 4),
                padding = new RectOffset(6, 6, 6, 6)
            };

            readOnlyFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 11,
                normal = { background = MakeTexture(new Color(0.12f, 0.14f, 0.18f, 0.8f)), textColor = new Color(0.9f, 0.93f, 0.96f) }
            };
        }

        public static Texture2D MakeTexture(Color color)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Repeat
            };
            tex.SetPixel(0, 0, color);
            tex.Apply(false, true);
            return tex;
        }
    }
}