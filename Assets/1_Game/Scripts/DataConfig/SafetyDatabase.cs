#if UNITY_EDITOR
using _1_Game.Scripts.Util;
using UnityEditor;
using UnityEngine;

namespace _1_Game.Scripts.DataConfig
{
    [InitializeOnLoad]
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
        
        [MenuItem("Tools/Refresh Database")]
        public static void RefreshDatabase()
        {
            SafetyDB.RefreshDatabase();
        }
        
        private static void LoadAsset()
        {
            string assetPath = "Assets/1_Game/Resources/DataBase.asset";
            SafetyDB = AssetDatabase.LoadAssetAtPath<GameDataBase>(assetPath);
        
            if (SafetyDB == null)
            {
                Debug.LogError($"Failed to load asset at {assetPath}");
            }
        }
    }
}
#endif