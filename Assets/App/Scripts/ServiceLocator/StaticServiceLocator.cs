using System;
using System.Collections.Generic;

namespace App.Scripts.ServiceLocator
{

    public interface IGameService {}
    
    public static class StaticServiceLocator
    {
        private static readonly Dictionary<Type, IGameService> Services = new Dictionary<Type, IGameService>();

        public static T Get<T>() where T : IGameService
        {
            foreach (var controller in Services.Values)
            {
                if (controller is T target) return target;
            }

            return default;
        }

        public static T Add<T>(T type) where T : IGameService
        {
            var t = typeof(T);
            if (Services.ContainsKey(t))
            {
                Services.Remove(t);
            }

            Services.Add(t, type);
            return type;
        }
    }
}