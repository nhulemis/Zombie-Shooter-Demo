using System;
using Sirenix.OdinInspector;

namespace Script.GameData
{
    public class BaseConfig : SerializedScriptableObject, IComparable<BaseConfig>
    {
        public string id;
        public int CompareTo(BaseConfig obj)
        {
            return string.Compare(id, obj.id, StringComparison.Ordinal);
        }
    }
}