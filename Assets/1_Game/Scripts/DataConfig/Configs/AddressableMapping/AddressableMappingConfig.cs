using System;
using System.Collections;
using System.Collections.Generic;
using _1_Game.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Script.GameData.AddressableMapping
{
    public class AddressableMappingConfig : BaseConfig
    {
        public List<AddressableMappingData> addressableMappingData = new List<AddressableMappingData>();
        
        public AssetReference GetPathId<T>()
        {
            var type = typeof(T);
            var mappingData = addressableMappingData.Find(data => data.type == type.ToString());
            return mappingData?.addressable;
        }
    }
    
    [Serializable]
    public class AddressableMappingData
    {
        [ValueDropdown("Internal_GetTypes")]
        public string type;
        public AssetReference addressable;

        private IEnumerable Internal_GetTypes()
        {
            var types = TypeHelper.GetTypesInFolder("_1_Game.Scripts");
            var result = new ValueDropdownList<string>();
            foreach (var type in types)
            {
                result.Add(type.Name, type.ToString());
            }
            return result;
        }
    }
}