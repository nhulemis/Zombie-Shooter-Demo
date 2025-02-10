#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _1_Game.Scripts.DataConfig
{
    [InitializeOnLoad]
    public static class SafetyDatabase
    {
        public static GameDataBase SafetyDB;
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