using _1_Game.Systems.Character;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _1_Game.Scripts.Systems.WeaponSystem.DamageEffect
{
    public interface IDameEffectAction
    {
        public AssetReference EffectPrefab { get; }
        public float EffectDuration { get; }
        void ApplyEffect(CharacterActor effectPosition);
    }
}