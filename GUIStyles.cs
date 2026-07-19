using UnityEngine;

namespace Poppers_server_notifier
{
    public static class GUIStyles
    {
        public static GUIStyle Window;
        public static GUIStyle Header;
        public static GUIStyle readOnlyFieldStyle;

        public static void Initialize()
        {
            if (Window != null)
                return;

            Window = new GUIStyle(GUI.skin.window);
            Window.normal.background = MakeTexture(new Color(0, 0, 0, .7f));
            Window.normal.textColor = Color.clear;
            Window.onNormal.textColor = Color.clear;

            Header = new GUIStyle(GUI.skin.box);
            Header.normal.background = MakeTexture(new Color(.345f, .396f, .949f));
            Header.normal.textColor = Color.white;
            Header.fontStyle = FontStyle.Bold;
            Header.alignment = TextAnchor.MiddleCenter;

            readOnlyFieldStyle = new GUIStyle(GUI.skin.textField);
            readOnlyFieldStyle.normal.background = MakeTexture(new Color(0.15f, 0.15f, 0.15f));
            readOnlyFieldStyle.normal.textColor = Color.gray;
        }

        private static Texture2D MakeTexture(Color color)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }
    }
}