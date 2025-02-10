using System;
using System.Collections.Generic;
using _1_Game.Scripts.Util;
using Sirenix.OdinInspector;

namespace Script.GameData.UIConfigs
{
    public class UIConfig : BaseConfig
    {
        [TableList] public List<UIDataConfig> UIDataConfigs = new List<UIDataConfig>();

        public UIDataConfig Get(string uiName)
        {
            return UIDataConfigs.Find(data => data.id == uiName);
        }
    }

    [Serializable]
    public class UIDataConfig
    {
        [ReadOnly] public string id;
        [ReadOnly] public string viewGroup;
        [ReadOnly] public UIBuilder builder;
        [ReadOnly] public string prefabPath;
    }
}