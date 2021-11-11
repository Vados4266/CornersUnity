using System;
using System.Collections.Generic;

namespace App.Scripts.Tools
{
    /// <summary>
    /// Single instance container
    /// </summary>
    public static class SIContainer
    {
        private static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static T Get<T>()
        {
            foreach (var controller in Instances.Values)
            {
                if (controller is T target) return target;
            }

            return default;
        }

        public static T Add<T>(T type)
        {
            var t = typeof(T);
            if (Instances.ContainsKey(t))
            {
                Instances.Remove(t);
            }

            Instances.Add(t, type);
            return type;
        }
    }
}