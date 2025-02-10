using System;

namespace _1_Game.Scripts.Util
{
    public class Locator <T>
    {
        private static T _instance;
        public static T Instance => _instance;
        public static void Set(T instance) => _instance = instance;

        public static void Release()
        {
            if (_instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _instance = default;
        }
    }
}