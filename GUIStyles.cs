using UnityEngine;

namespace Poppers_server_notifier
{
    public static class GUIStyles
    {
        public static GUIStyle Window;
        public static GUIStyle Header;
        public static GUIStyle KeyStyle;
        public static GUIStyle ValueStyle;
        public static GUIStyle readOnlyFieldStyle;

        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            Window = new GUIStyle(GUI.skin.window);
            Window.normal.background = MakeTexture(new Color(0.05f, 0.05f, 0.07f, 0.94f));
            Window.normal.textColor = Color.clear;
            Window.onNormal.textColor = Color.clear;

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
                fontStyle = FontStyle.Normal,
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

            readOnlyFieldStyle = new GUIStyle(GUI.skin.textField);
            readOnlyFieldStyle.normal.background = MakeTexture(new Color(0.12f, 0.14f, 0.18f, 0.8f));
            readOnlyFieldStyle.normal.textColor = new Color(0.9f, 0.93f, 0.96f);
            readOnlyFieldStyle.fontSize = 11;
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