using _1_Game.Scripts.Util;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _1_Game.Scripts.DataConfig
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class SafetyDatabase
    {
#if UNITY_EDITOR
        public static GameDataBase SafetyDB;
        #else
        public static GameDataBase SafetyDB => Locator<GameDataBase>.Instance;
#endif
        static SafetyDatabase()
        {
            LoadAsset();
        }

#if UNITY_EDITOR
        [MenuItem("Tools/Refresh Database")]
#endif
        public static void RefreshDatabase()
        {
#if UNITY_EDITOR
            SafetyDB.RefreshDatabase();
#endif
        }
        
        private static void LoadAsset()
        {
#if UNITY_EDITOR
            string assetPath = "Assets/1_Game/Resources/DataBase.asset";
            SafetyDB = AssetDatabase.LoadAssetAtPath<GameDataBase>(assetPath);
        
            if (SafetyDB == null)
            {
                Debug.LogError($"Failed to load asset at {assetPath}");
            }
#endif
        }
    }
}
