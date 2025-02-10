using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Script.GameData
{
    public class CharacterConfig : BaseConfig
    {
        [TableList]
        public List<CharacterData> CharacterDataList = new();

        public CharacterData Get(string gameObjectName)
        {
            return CharacterDataList.Find(c => c.id == gameObjectName);
        }

    }

    [Serializable]
    public class CharacterData
    {
        [ReadOnly]
        public string id;
        [ReadOnly]
        public string path;
    }
}