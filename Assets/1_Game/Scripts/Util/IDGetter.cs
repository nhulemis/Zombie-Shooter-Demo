#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _1_Game.Scripts.DataConfig;
using Script.GameData;

namespace _1_Game.Scripts.Util
{
   

    public static class IDGetter
    {
        public static IEnumerable GetUIGroupName()
        {
            return new List<string>(){"GameView", "SplashView"};
        }
        
        public static IEnumerable GetCharacterConfig()
        {
            var characterConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>();
            return characterConfig.CharacterDataList.Select(c => c.id);
        }
    }
}
#endif