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
#if !UNITY_EDITOR
            return new List<string>();
#endif
            var characterConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>();
            var result = new List<string>() { "none" };
            result.AddRange(characterConfig.CharacterDataList.Select(c => c.id));
            return result;
        }
        
        public static IEnumerable GetWeaponConfigs()
        {
#if !UNITY_EDITOR
            return new List<string>();
#endif
            var weaponConfig = SafetyDatabase.SafetyDB.Get<WeaponConfig>();
            return weaponConfig.weaponDataSets.Select(c => c.id);
        }

        public static IEnumerable GetAnimationLayerMappingConfigs()
        {
#if !UNITY_EDITOR
            return new List<string>();
#endif
            var animationLayerMappingConfig = SafetyDatabase.SafetyDB.Get<AnimationLayerMappingConfig>();
            return animationLayerMappingConfig.GetLayerNames();
        }

        public static IEnumerable GetParameterAnimationNames()
        {
#if !UNITY_EDITOR
            return new List<string>();
#endif
            var animationLayerMappingConfig = SafetyDatabase.SafetyDB.Get<AnimationLayerMappingConfig>();
            return animationLayerMappingConfig.GetParameterNames();
        }
        
        public static IEnumerable GetAnimationClips()
        {
#if !UNITY_EDITOR
            return new List<string>();
#endif
            var animationLayerMappingConfig = SafetyDatabase.SafetyDB.Get<AnimationLayerMappingConfig>();
            return animationLayerMappingConfig.GetAllClips();
        }
    }
}
