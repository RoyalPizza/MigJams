using UnityEngine;

namespace Pizza
{
    public static class PizzaOnGUI
    {
        public static GUIStyle NormalButton;

        public static GUIStyle HeaderLabel;
        public static GUIStyle NormalLabel;
        public static GUIStyle RedLabel;

        static PizzaOnGUI()
        {
            NormalButton = new GUIStyle(GUI.skin.button);
            NormalButton.fontSize = 20;

            HeaderLabel = new GUIStyle(GUI.skin.label);
            HeaderLabel.fontSize = 22;
            HeaderLabel.fontStyle = FontStyle.Bold;

            NormalLabel = new GUIStyle(GUI.skin.label);
            NormalLabel.fontSize = 18;

            RedLabel = new GUIStyle(GUI.skin.label);
            RedLabel.fontSize = 18;
            RedLabel.normal.textColor = Color.red;
        }
    }
}