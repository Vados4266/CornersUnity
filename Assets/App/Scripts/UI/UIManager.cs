using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        private static readonly List<UIWidget> Widgets = new List<UIWidget>();

        private void Awake()
        {
            var widgets = FindObjectsOfType<UIWidget>(true);
            foreach (var widget in widgets)
            {
                Add(widget);
            }
        }

        public static void Add(UIWidget widget)
        {
            if (!Widgets.Contains(widget)) Widgets.Add(widget);
        }

        public static T GetWidget<T>()
        {
            foreach (var widget in Widgets)
            {
                if (widget is T target) return target;
            }

            return default;
        }

        public static void CloseAll()
        {
            foreach (var widget in Widgets)
            {
                widget.Close();
            }
        }
    }
}