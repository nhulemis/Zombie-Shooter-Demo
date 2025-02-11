#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _1_Game.Scripts.DataConfig;
using Script.GameData;
using Script.GameData.AnimationLayerMapping;
using Script.GameData.Weapon;

namespace _1_Game.Scripts.Util
{
   

    public static class IDGetter
    {
        public static IEnumerable GetUIGroupName()
        {
            return new List<string>(){"GameView", "SplashView"};
        }
        
        public static IEnumerable GetCharacterConfigs()
        {
            var characterConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>();
            return characterConfig.CharacterDataList.Select(c => c.id);
        }
        
        public static IEnumerable GetWeaponConfigs()
        {
            var weaponConfig = SafetyDatabase.SafetyDB.Get<WeaponConfig>();
            return weaponConfig.weaponDataSets.Select(c => c.id);
        }

        public static IEnumerable GetAnimationLayerMappingConfigs()
        {
            var animationLayerMappingConfig = SafetyDatabase.SafetyDB.Get<AnimationLayerMappingConfig>();
            return animationLayerMappingConfig.GetLayerNames();
        }
    }
}
#endif