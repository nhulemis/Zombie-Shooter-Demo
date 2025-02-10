using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Script.GameData
{
    public class CharacterConfig : BaseConfig
    {
        [ListDrawerSettings(CustomAddFunction = "AddNewConfig", CustomRemoveElementFunction = "RemoveDataTable")]
        public List<CharacterDataConfig> CharacterDataList = new();

        protected override IList Items => CharacterDataList;

        public CharacterDataConfig Get(string gameObjectName)
        {
            return CharacterDataList.Find(c => c.id == gameObjectName);
        }
    }
    
}