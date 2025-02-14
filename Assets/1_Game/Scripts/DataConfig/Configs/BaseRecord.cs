using System;
using Sirenix.OdinInspector;

namespace Script.GameData
{
    [Serializable]
    public class BaseRecord : SerializedScriptableObject
    {
        [ReadOnly]
        public string id;
    }
}