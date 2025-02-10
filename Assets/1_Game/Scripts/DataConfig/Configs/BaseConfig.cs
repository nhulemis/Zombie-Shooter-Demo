using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Script.GameData
{
    public class BaseConfig : SerializedScriptableObject, IComparable<BaseConfig>
    {
        public string id;
        protected virtual IList Items { get; }
        public int CompareTo(BaseConfig obj)
        {
            return string.Compare(id, obj.id, StringComparison.Ordinal);
        }
        
#if UNITY_EDITOR
        public virtual void RemoveDataTable(BaseRecord record)
        {
            if(record == null) return;
            Items.Remove(record);
            AssetDatabase.RemoveObjectFromAsset(record);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
        }
        public virtual void AddNewConfig()
        {
            CreateNewItemWindow.Show((newItemId) =>
            {
                var type = Items.GetType().GetGenericArguments().Single();
                var record = CreateInstance(type) as BaseRecord;
                record.name = newItemId;
                record.id = newItemId.Replace(" ", "_");
                Items.Add(record);
                AssetDatabase.AddObjectToAsset(record, this);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                return string.Empty;
            }, "ID");
        }
#endif
    }
}