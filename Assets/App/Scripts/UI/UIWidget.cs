using UnityEngine;

namespace App.Scripts.UI
{
    public class UIWidget: MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.Add(this);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        protected T GetWidget<T>() => UIManager.GetWidget<T>();
    }
}