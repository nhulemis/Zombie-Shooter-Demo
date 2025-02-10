using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Script.GameData;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _1_Game.Scripts.DataConfig
{
    [CreateAssetMenu(fileName = "DataBase", menuName = "GameData/DataBase", order = 1)]
    public class GameDataBase : SerializedScriptableObject
    {
        [FolderPath]
        public string filesPath;
        public List<BaseConfig> BaseConfigs = new List<BaseConfig>();
        
        public T Get<T>() where T : BaseConfig
        {
            foreach (var config in BaseConfigs)
            {
                if (config is T baseConfig)
                {
                    return baseConfig;
                }
            }
            return null;
        }

#if UNITY_EDITOR
        [Button]
        public void RefreshDatabase()
        {
            EditorUtility.SetDirty(this);
            var baseType = typeof(BaseConfig);
            var assembly = Assembly.GetAssembly(baseType);
            var types = assembly.GetTypes();

            var classes = types.Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).ToList();

            foreach (var type in classes)
            {
                if (BaseConfigs.Exists(c => c.GetType() == type)) continue;
                CreateAsset(type);
            }
            
            BaseConfigs.Sort();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void CreateAsset(Type type)
        {
            var asset = ScriptableObject.CreateInstance(type);
            string assetPath = $"{filesPath}/{type.Name}.asset";
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            BaseConfigs.Add((BaseConfig)asset);
        }
#endif
    }
}