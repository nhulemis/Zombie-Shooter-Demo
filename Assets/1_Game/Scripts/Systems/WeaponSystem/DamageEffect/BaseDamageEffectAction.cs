using System;
using _1_Game.Systems.Character;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _1_Game.Scripts.Systems.WeaponSystem.DamageEffect
{
    [Serializable]
    public class BaseDamageEffectAction : IDameEffectAction
    {
        [SerializeField]
        private AssetReference _effectPrefab;
        [SerializeField]
        private float _effectDuration;
        public AssetReference EffectPrefab => _effectPrefab;
        public float EffectDuration => _effectDuration;
        public virtual void ApplyEffect(CharacterActor effectPosition){}
    }
}